using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rally.RestApi;
using RallyToolsCore.Properties;

namespace TrackGearLibrary.Rally
{
	public partial class SignDefects : Form
	{
		readonly string[] _defectIds;

		public SignDefects(string[] defectIds)
		{
			InitializeComponent();

			_defectIds = defectIds;
		}

		void SignDefects_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(Settings.Default.RallyUser))
			{
				new RallySettings().ShowDialog();
			}

			foreach (var defectId in _defectIds)
			{
				var lvi = new ListViewItem(defectId);
				lvi.Checked = true;
				lvi.Tag = new Artifact { FormattedID = defectId };
				lvi.SubItems.Add("");
				lvi.SubItems.Add("");
				lvi.SubItems.Add("");
				listViewArtifacts.Items.Add(lvi);
			}

			var resolveTask = Task.Factory
				.StartNew(() => ResolveArtifacts())
			;

			simplifiedBackgroundOperation.Text = "Resolved";
			simplifiedBackgroundOperation
				.Track(resolveTask, "Resolving artifacts URLs")
			;

			resolveTask
				.ContinueWith(t => {

					if (t.IsCanceled)
					{
						Close();
						return;
					}

					foreach (ListViewItem lvi in listViewArtifacts.Items)
					{
						if (t.IsFaulted)
						{
							lvi.BackColor = Color.Tomato;
							lvi.SubItems[1].Text = "Error :(";
							continue;
						}

						var tagArt = (Artifact)lvi.Tag;

						var updatedArtifact = t.Result.FirstOrDefault(a => a.FormattedID.ToLowerInvariant() == tagArt.FormattedID.ToLowerInvariant());
						if (updatedArtifact == null)
						{
							lvi.BackColor = Color.Tomato;
							lvi.Checked = false;
							lvi.SubItems[1].Text = "Error :(";
							continue;
						}

						lvi.Tag = updatedArtifact;
						lvi.SubItems[1].Text = updatedArtifact.Name;
						lvi.SubItems[2].Text = updatedArtifact.Parent.Name ?? "<n/a>";
					}

					buttonDoSign.Enabled = true;

				}, TaskScheduler.FromCurrentSynchronizationContext())
			;
		}

		Artifact[] ResolveArtifacts()
		{
			var restApi = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword
				//, proxy: new WebProxy("localhost:8888", false)
			);

			var request = new Request("artifact");
			request.Fetch = new List<string> { "FormattedID", "Discussion", "Name", "Requirement", "WorkProduct" };

			// build query for many items
			var queries = _defectIds
				.Select(id => new Query("FormattedID", Query.Operator.Equals, id))
				.ToArray()
			;

			var query = queries[0];
			for (var i = 1; i < queries.Length; i++)
			{
				query = query.Or(queries[i]);
			}

			request.Query = query;

			// perfrom query
			var resp = restApi.Query(request);

			if(!resp.Success)
			{
				if (resp.Errors.Count > 0)
					throw new ApplicationException(resp.Errors[0]);

				throw new ApplicationException("Unexpected error");
			}

			return resp.Results.Select(a => new Artifact(a)).ToArray();
		}

		void buttonDoSign_Click(object sender, EventArgs e)
		{
			buttonDoSign.Enabled = false;

			var sc = SynchronizationContext.Current;
			var arts = listViewArtifacts.CheckedItems.Cast<ListViewItem>().Select(lvi => (Artifact)lvi.Tag).ToArray();

			var task = Task.Factory
				.StartNew(() => DoSign(arts, sc))
			;
			
			backgroundOperation
				.TrackWithCancellation(task)
			;
		}

		int DoSign(IEnumerable<Artifact> artifacts, SynchronizationContext sc)
		{
			var restApi = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword
				//, proxy: new WebProxy("localhost:8888", false)
				);

			foreach (var tagArt in artifacts)
			{
				var toCreate = new DynamicJsonObject();
				toCreate["Text"] = "%FIX_CODE_REVIEW_PASSED%";
				toCreate["Artifact"] = tagArt.Reference;
				var createResult = restApi.Create("ConversationPost", toCreate);

				// update status
				sc.Post(_ => {
					var lvi = listViewArtifacts.Items.Cast<ListViewItem>().FirstOrDefault(l => l.Tag == tagArt);
					if (!createResult.Success)
					{
						lvi.BackColor = Color.Tomato;

						if (createResult.Errors.Count > 0)
							lvi.SubItems[1].Text = createResult.Errors[0];
						else
							lvi.SubItems[1].Text = "Unexpected error";
					}
					else
					{
						lvi.BackColor = Color.LightGreen;
						lvi.SubItems[3].Text = "✔";
					}
				}, null);
			}

			return 0;
		}
	}
}
