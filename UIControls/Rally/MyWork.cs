using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using Rally.RestApi;
using RallyToolsCore.Properties;
using UIControls;
using UIControls.Core;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using System.Diagnostics;

namespace TrackGearLibrary.Rally
{
	public partial class DailyReport : Form
	{
		public DailyReport()
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.DailyReport);

			InitializeComponent();

			// smart select preset. if it is earlier than 11 o'clock, than seems we want generate daily report for previous day, so select 24 hours
			if(DateTimeOffset.Now.TimeOfDay <= new TimeSpan(0, 11, 0, 0))
				comboBoxDatePreset.SelectedIndex = 1;
			else
				comboBoxDatePreset.SelectedIndex = 0;
		}

		class ChangedArtifact
		{
			public Artifact WorkItem;
			public DynamicJsonObject Artifact;
			public List<DynamicJsonObject> Revisions;

			public double ToDo = -1;
			public double Estimate = -1;

			public double ToDoDiff;
			public double EstimateDiff;

			public bool BecomeCompleted;
		}

		void DailyReport_Load(object sender, EventArgs e)
		{
			var cache = LoadCache();

			if (cache != null && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
				ApplyChanges(cache);
			else
				RefreshChanges();
		}

		void buttonLoadChanges_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.DailyReportClickLoad);
			RefreshChanges();
		}

		void RefreshChanges()
		{
			StartLoadChanges()
				.LockControls(dateTimePickerFrom, dateTimePickerTo, listViewChanged, buttonLoadChanges, comboBoxDatePreset, buttonGenerate)
				.ContinueWith(t => ApplyChanges(t.Result), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
			;
		}

		static readonly Regex RevChangeRegex = new Regex(@"^(?<type>STATE|TO DO|ESTIMATE) changed from \[(?<from>.+?)\] to \[(?<to>.+?)\]");

		void ApplyChanges(IEnumerable<ChangedArtifact> changes)
		{
			listViewChanged.Items.Clear();
			foreach (var change in changes.OrderBy(c => c.WorkItem.Owner.Name ?? "<n/a>"))
			{
				// check if artifact time was modified
				foreach (var revision in change.Revisions)
				{
					// not very correct. ',' can be in change text
					var revDescs = ((string)revision["Description"]).Split(',');

					foreach (var desc in revDescs)
					{
						try
						{
							var m = RevChangeRegex.Match(desc.Trim());

							if (m.Success)
							{
								var type = m.Groups["type"].Value;
								if (type == "STATE")
								{
									var to = m.Groups["to"].Value;
									change.BecomeCompleted = (to == "Completed");
								}
								else if (type == "TO DO")
								{
									double from, to;
									change.ToDoDiff += GetTimeDiff(m, out from, out to);
								}
								else if (type == "ESTIMATE")
								{
									double from, to;
									change.EstimateDiff += GetTimeDiff(m, out from, out to);
								}
							}
						}
						catch
						{
						}
					}
				}

				var artifact = new Artifact(change.Artifact);

				var lvi = new ListViewItem {
					Tag = change
				};

				lvi.ToolTipText = string.Join("\n---\n", change.Revisions.Select(r => r["Description"]));

				lvi.Text = artifact.Project.Name ?? "<n/a>";
				lvi.SubItems.Add(artifact.Owner.Name ?? "<n/a>");
				lvi.SubItems.Add(artifact.Parent.Name ?? "<n/a>");
				lvi.SubItems.Add(artifact.FormattedID);
				lvi.SubItems.Add(artifact.Name);
				listViewChanged.Items.Add(lvi);

				var state = Artifact.TryGetMember<string>(change.Artifact, "State");
				{
					lvi.SubItems.Add(state);
					if (state == "Completed")
						lvi.ForeColor = Color.Gray;
				}

				{
					var estimate = Artifact.TryGetMember<double>(change.Artifact, "Estimate", -1);
					var todo = Artifact.TryGetMember<double>(change.Artifact, "ToDo", -1);

					change.Estimate = estimate;
					change.ToDo = todo;

					var done = 0;
					if (estimate >= 0 && todo > 0)
						done = (int)(100 * (estimate - todo) / estimate);

					if (done > 0)
					{
						lvi.SubItems.Add(string.Format("{0:0}%", done));
					}
					else
					{
						lvi.SubItems.Add("");
					}
				}

				var c = GetShortDescriptiveChanges(change);
				lvi.SubItems.Add(c);

				var ownerMe = change.WorkItem.Owner.ObjectID == _currentUser["ObjectID"];

				lvi.Selected =
					// has some activity
					!string.IsNullOrWhiteSpace(c)
					// was completed
					|| change.BecomeCompleted
					// mine and in progress
					|| (state == "In-Progress" && ownerMe)
					// created in already completed state
					|| (state == "Completed" && change.Revisions.Count == 1)
				;
			}
			listViewChanged.Focus();
		}

		static string GetShortDescriptiveChanges(ChangedArtifact change)
		{
			var sb = new StringBuilder();

			if (Math.Abs(change.ToDoDiff) > 0.001)
			{
				if (sb.Length > 0)
					sb.Append("; ");

				sb.AppendFormat("Work: {0:+#.##;-#.##} H.", -change.ToDoDiff + change.EstimateDiff);
			}

			if (Math.Abs(change.EstimateDiff) > 0.001)
			{
				if (sb.Length > 0)
					sb.Append("; ");

				sb.AppendFormat("Est.: {0:+#.##;-#.##} H.", change.EstimateDiff);
			}

			return sb.ToString();
		}

		static string GetHtmlDescriptiveChanges(ChangedArtifact change)
		{
			var sb = new StringBuilder();

			if (change.BecomeCompleted)
			{
				sb.Append("Become Completed");
			}

			if (Math.Abs(change.ToDoDiff) > 0.001)
			{
				if (sb.Length > 0)
					sb.Append("<br />");

				sb.AppendFormat("Done: {0:+#.##;-#.##} Hours", -change.ToDoDiff + change.EstimateDiff);
			}

			if (Math.Abs(change.EstimateDiff) > 0.001)
			{
				if (sb.Length > 0)
					sb.Append("<br />");

				sb.AppendFormat("Estimate: {0:+#.##;-#.##} Hours", change.EstimateDiff);
			}

			return sb.ToString();
		}

		static double GetTimeDiff(Match m, out double fromN, out double toN)
		{
			fromN = 0;
			toN = 0;

			try
			{
				var from = m.Groups["from"].Value;
				var to = m.Groups["to"].Value;

				if (@from.EndsWith(" Hours") && to.EndsWith(" Hours"))
				{
					@from = @from.Substring(0, @from.Length - " Hours".Length);
					to = to.Substring(0, to.Length - " Hours".Length);

					fromN = double.Parse(@from, CultureInfo.InvariantCulture);
					toN = double.Parse(to, CultureInfo.InvariantCulture);

					return toN - fromN;
				}
			}
			catch
			{
			}
			return 0;
		}

		#region Cache management

		static void UpdateCache(IEnumerable<ChangedArtifact> chnages)
		{
			var serializable = chnages.Select(a => new { Artifact = a.Artifact.ToDictionary(), Revisions = a.Revisions.Select(r => r.ToDictionary()).ToList() });
			var dict = new Dictionary<string, object>();
			dict["changes"] = serializable;

			var cache = JsonConvert.SerializeObject(dict, Formatting.Indented);

			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.AppDataDirName);
			Directory.CreateDirectory(dir);

			var path = Path.Combine(dir, "cache-dr.json");

			File.WriteAllText(path, cache);
		}

		static IEnumerable<ChangedArtifact> LoadCache()
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.AppDataDirName, "cache-dr.json");

			if (!File.Exists(path))
				return null;

			try
			{
				var json = File.ReadAllText(path);

				var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);

				return ((ArrayList)dict["changes"])
					.Cast<Dictionary<string, object>>()
					.Select(d => new ChangedArtifact { Artifact = new DynamicJsonObject((Dictionary<string, object>)d["Artifact"]),
						Revisions = ((ArrayList)d["Revisions"])
							.Cast<Dictionary<string, object>>()
							.Select(dd => new DynamicJsonObject(dd))
							.ToList(),
						WorkItem = new Artifact(new DynamicJsonObject((Dictionary<string, object>)d["Artifact"]))
					})
					.ToList()
				;
			}
			catch (Exception)
			{
			}

			return null;
		}
		#endregion

		#region Loading data

		Task<List<ChangedArtifact>> StartLoadChanges()
		{
			var cts = new CancellationTokenSource();

			var task = Task.Factory.StartNew(() => LoadChanges(dateTimePickerFrom.Value, dateTimePickerTo.Value, cts.Token), cts.Token);

			return backgroundOperation
				.TrackWithCancellation(task, cts)
			;
		}

		DynamicJsonObject _currentUser;
		List<ChangedArtifact> LoadChanges(DateTime fromTime, DateTime toTime, CancellationToken ct)
		{
			var toS = toTime.ToString("yyyy-MM-ddTHH:mm:ss");
			var fromS = fromTime.ToString("yyyy-MM-ddTHH:mm:ss");

			var client = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword, cancellationToken: ct);

			if (_currentUser == null)
			{
				backgroundOperation.SetProgress("Get current user ...");
				if (string.IsNullOrWhiteSpace(textBoxUser.Text) || textBoxUser.Tag != null)
				{
					_currentUser = client.GetCurrentUser();
				}
				else
				{
					var query = new Query("(UserName = " + textBoxUser.Text + ")");
					var resp = client.Query(new Request("user") { Query = query });
					_currentUser = resp.Results.First();
				}
			}

			// retrieve revisions
			var requestRevs = new Request("revision");
			requestRevs.Limit = Int32.MaxValue;
			requestRevs.Fetch = new List<string> { "Description", "RevisionHistory", "CreationDate" };
			requestRevs.Query = new Query("User", Query.Operator.Equals, _currentUser["UserName"])
				.And(new Query("CreationDate", Query.Operator.GreaterThanOrEqualTo, fromS))
				.And(new Query("CreationDate", Query.Operator.LessThanOrEqualTo, toS))
			;

			backgroundOperation.SetProgress("Load revisions ...");

			var resultRevs = client.Query(requestRevs);

			var revHists = resultRevs.Results.Cast<DynamicJsonObject>()
				.GroupBy(r => (string)r["RevisionHistory"]["_refObjectUUID"])
				.ToDictionary(g => g.Key, g => g.Select(r => (DynamicJsonObject)r).ToList())
			;

			// check cancellation
			ct.ThrowIfCancellationRequested();

			// query artifacts
			var requestArtifacts = new Request("artifact");
			requestArtifacts.Limit = Int32.MaxValue;
			requestArtifacts.Fetch = new List<string> { "FormattedID", "Name", "RevisionHistory", "LastUpdateDate", "Requirement", "WorkProduct", "ObjectID", "Owner", "Project" };
			requestArtifacts.Query = new Query("LastUpdateDate", Query.Operator.GreaterThanOrEqualTo, fromS)
				.And(new Query("LastUpdateDate", Query.Operator.LessThanOrEqualTo, toS))
			;

			var artUuid2Revs = new Dictionary<string, List<DynamicJsonObject>>();
			var artUuid2Art = new Dictionary<string, DynamicJsonObject>();

			backgroundOperation.SetProgress("Load artifacts ...");

			var resultArts = client.Query(requestArtifacts);

			foreach (var art in resultArts.Results)
			{
				// can't inject in Query ....
				var type = (string)art["_type"];
				if (type != "Task" && type != "Defect")
					continue;

				var histUuid = (string)art["RevisionHistory"]["_refObjectUUID"];
				if (!revHists.ContainsKey(histUuid))
					continue;

				List<DynamicJsonObject> artifactRevs;
				if (!artUuid2Revs.TryGetValue((string)art["_refObjectUUID"], out artifactRevs))
				{
					artifactRevs = new List<DynamicJsonObject>();
					artUuid2Revs[(string)art["_refObjectUUID"]] = artifactRevs;
				}

				artifactRevs.AddRange(revHists[histUuid]);
				artUuid2Art[(string)art["_refObjectUUID"]] = art;
			}

			foreach (var group in artUuid2Art.GroupBy(kvp => new Artifact(kvp.Value).Type).ToArray())
			{
				ct.ThrowIfCancellationRequested();

				backgroundOperation.SetProgress(string.Format("Loading group of '{0}' ...", group.Key));

				var allIds = group.Select(a => new Artifact(a.Value).ObjectID.ToString(CultureInfo.InvariantCulture)).ToArray();
				var query = allIds
					.Skip(1)
					.Aggregate(new Query("ObjectId", Query.Operator.Equals, allIds.First()), (current, id) => current.Or(new Query("ObjectId", Query.Operator.Equals, id)))
				;

				var r = new Request(group.First().Value["_type"]);
				r.Limit = Int32.MaxValue;
				r.Query = query;
				var qr = client.Query(r);

				foreach (var result in qr.Results)
				{
					if (result["_type"] == "Task")
					{
						artUuid2Art[result["_refObjectUUID"]]["Estimate"] = (double)result["Estimate"];
						artUuid2Art[result["_refObjectUUID"]]["ToDo"] = (double)result["ToDo"];
						artUuid2Art[result["_refObjectUUID"]]["State"] = (string)result["State"];
					}
					else if (result["_type"] == "Defect")
					{
						//var additionalInfo = client.GetByReference(art.Reference);

						//artUuid2Art[kvp.Key]["Estimate"] = additionalInfo["Estimate"];
						//artUuid2Art["ToDo"] = additionalInfo["ToDo"];
					}
				}
			}

			var changes = artUuid2Revs
				.Select(kvp => new ChangedArtifact { Artifact = artUuid2Art[kvp.Key], Revisions = kvp.Value, WorkItem = new Artifact(artUuid2Art[kvp.Key]) })
				.ToList()
			;

			UpdateCache(changes);

			return changes;
		}
		#endregion

		void buttonGenerate_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.DailyReportClickGenerate);
			var selectedArtifacts = listViewChanged.SelectedItems
				.Cast<ListViewItem>()
				.Select(lvi => lvi.Tag as ChangedArtifact)
				.Select(change => new { Artifact = new Artifact(change.Artifact), Change = change, RawArtifact = change.Artifact })
				.ToList()
			;

			var groupedArtifacts = selectedArtifacts
				.GroupBy(a => {
					if (a.Artifact.Parent != null && a.Artifact.Parent.Type == ArtifactType.Defect)
					{
						if (a.Artifact.Parent.Parent == null)
							return null;

						return a.Artifact.Parent.Parent.Name;
					}

					if (a.Artifact.Parent == null)
						return null;

					return a.Artifact.Parent.Name;
				})
				.ToArray()
			;

			var sb = new StringBuilder();

			sb.AppendLine(@"<head><style>
td, th {
	border: 1px solid black;
	padding: 3px;
}
table {
	border-collapse: collapse;
	table-layout: fixed;
}
.parent-col {
	width: 200px;
}
td.parent-col {
	font-size: 70%;
	color: #B8B894
}
.task-col {
}
.state-col {
	width: 100px;
}
td.state-col {
	font-size: 70%;
	width: 100px;
}
.notes-col {
	width: 400px;
}
</style></head>
");

			sb.AppendLine("<TABLE border=1>");
			sb.AppendLine("<tr>");
			sb.AppendLine("<th class='parent-col'>Parent (US/DE)</th>");
			sb.AppendLine("<th class='task-col'>Task / Defect</th>");
			sb.AppendLine("<th class='state-col'>State</th>");
			sb.AppendLine("<th class='notes-col'>Notes</th>");
			sb.AppendLine("</tr>");

			foreach (var group in groupedArtifacts)
			{
				sb.AppendLine("<tr>");
				sb.AppendFormat("<td class='parent-col' rowspan='{0}' style='word-wrap: break-word'>", group.Count());

				var groupArtifact = group.First().Change.WorkItem.Parent;
				if (groupArtifact.Parent != null && groupArtifact.Parent.Type == ArtifactType.Defect)
					groupArtifact = groupArtifact.Parent;

				sb.AppendFormat("<a href='{0}'>{1}</a>: {2}</td>",
					HttpUtility.UrlEncode(groupArtifact.Url),
					groupArtifact.FormattedID,
					HttpUtility.HtmlEncode(groupArtifact.Name)
				);

				sb.AppendFormat("</td>");
				var addTr = false;
				foreach (var change in group)
				{
					var artifact = change.Artifact;

					if (addTr)
					{
						sb.AppendLine("<tr>");
					}
					sb.AppendLine("<td class='task-col'>");
					sb.AppendFormat("<a href='{0}'>{1}</a>: {2}", artifact.Url, artifact.FormattedID, HttpUtility.HtmlEncode(artifact.Name));
					sb.AppendLine();
					if (artifact.Parent != null && artifact.Parent.Type == ArtifactType.Defect)
					{
						sb.AppendLine("<br/>");
						sb.AppendLine("of defect:<br/>");
						sb.AppendFormat("<a href='{0}'>{1}</a>: {2}", artifact.Parent.Url, artifact.Parent.FormattedID, HttpUtility.HtmlEncode(artifact.Parent.Name));
						sb.AppendLine();
					}

					sb.AppendLine("</td>");

					sb.AppendLine("<td class='state-col' align=center>");

					var state = Artifact.TryGetMember<string>(change.RawArtifact, "State");
					if(state == "Completed")
					{
						sb.AppendLine("Completed");
					}
					else if(state == "Defined")
					{
					}
					else if (state == "In-Progress")
					{
						if (change.Change.Estimate > 0 && change.Change.ToDo > 0)
						{
							var done = (int)(100 * (change.Change.Estimate - change.Change.ToDo) / change.Change.Estimate);
							if (done > 0)
							{
								sb.AppendFormat("{0}%<br/>", done);
								sb.AppendFormat(@"
<table style='width: 100px; border-collapse: collapse; table-layout: fixed;' >
	<tr style='padding: 0px;'>
		<td style='width: {0}px; background-color: lightgreen; padding: 0px; font-size: 50%;'>&nbsp;</td>
		<td style='width: {1}px; background-color: gray padding: 0px; font-size: 50%;'>&nbsp;</td>
	</tr>
</table>
", done, 100 - done);
							}
						}
						else
						{
							sb.AppendLine("In Progress");
						}
					}
					else
					{
						sb.AppendLine(state);
					}

					sb.AppendLine("</td>");

					var c = GetHtmlDescriptiveChanges(change.Change);

					sb.AppendFormat("<td class='notes-col'>{0}</td>", c);

					sb.AppendLine("</tr>");
					addTr = true;
				}
			}
			sb.AppendLine("</TABLE>");

			var body = sb.ToString();

			var app = new Microsoft.Office.Interop.Outlook.Application();

			Microsoft.Office.Interop.Outlook.MailItem msg;
			if (!string.IsNullOrWhiteSpace(Settings.Default.DailyReportTemplatePath))
			{
				msg = (Microsoft.Office.Interop.Outlook.MailItem)app.CreateItemFromTemplate(Settings.Default.DailyReportTemplatePath);
				msg.HTMLBody = BuildMailBody(body, msg.HTMLBody);
			}
			else
			{
				msg = (Microsoft.Office.Interop.Outlook.MailItem)app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
				msg.Subject = "Daily Report";
				msg.HTMLBody = BuildMailBody(body);
			}

			msg.Display();

			Clipboard.SetText(sb.ToString());
		}

		const string DefaultEmailTemplate = @"
<HTML>
Hello,
<br/>
<br/>
$$REPORT$$
<br/>
<br/>
</HTML>
";

		static string BuildMailBody(string report, string template = DefaultEmailTemplate)
		{
			if (template.Contains("$$REPORT$$"))
				return template.Replace("$$REPORT$$", report);

			return report;
		}

		void DailyReport_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				RefreshChanges();
			}
		}

		private void DailyReport_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				RefreshChanges();
			}
		}

		void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.DailyReportCopy);

			var selectedArtifacts = listViewChanged.SelectedItems
				.Cast<ListViewItem>()
				.Select(lvi => lvi.Tag as ChangedArtifact)
				.Select(change => new { Artifact = new Artifact(change.Artifact), Change = change })
				.ToList()
			;

			var groupedArtifacts = selectedArtifacts
				.GroupBy(a => a.Artifact.Parent.Name)
				.ToArray()
			;

			var sb = new StringBuilder();

			foreach (var group in groupedArtifacts)
			{
				if(sb.Length > 0)
					sb.AppendLine();

				sb.AppendLine(group.Key);
				foreach (var artifact in group)
				{
					sb.AppendFormat("	{0}: {1}", artifact.Artifact.FormattedID, artifact.Artifact.Name);
					sb.AppendLine();
				}
			}

			Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
		}

		bool _lockPresetChange;
		void comboBoxDatePreset_SelectedIndexChanged(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.DailyReportChangePreset, comboBoxDatePreset.SelectedIndex);

			_lockPresetChange = true;
			switch (comboBoxDatePreset.SelectedIndex)
			{
				case 0:
					// today
					dateTimePickerFrom.Value = DateTime.Now.Date;
					dateTimePickerTo.Value = DateTime.Now.Date + TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1);
					break;
				case 1:
					// 24 hours
					dateTimePickerFrom.Value = DateTime.Now - TimeSpan.FromDays(1);
					dateTimePickerTo.Value = DateTime.Now;
					break;
				case 2:
					// yesterday
					dateTimePickerFrom.Value = DateTime.Now.Date - TimeSpan.FromDays(1);
					dateTimePickerTo.Value = DateTime.Now.Date - TimeSpan.FromSeconds(1);
					break;
			}
			_lockPresetChange = false;
		}

		void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
		{
			if (!_lockPresetChange)
				comboBoxDatePreset.SelectedIndex = -1;
		}

		void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
		{
			if (!_lockPresetChange)
				comboBoxDatePreset.SelectedIndex = -1;
		}

		void openInRallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listViewChanged.SelectedItems
				.Cast<ListViewItem>()
				.Select(lvi => lvi.Tag as ChangedArtifact)
				.Select(c => new Artifact(c.Artifact).Url)
				.Where(u => u != null)
				.ToList()
				.ForEach(a => Process.Start(a))
			;
		}

		void textBoxUser_TextChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(textBoxUser.Text))
				_currentUser = null;
		}

		void textBoxUser_Enter(object sender, EventArgs e)
		{
			if (textBoxUser.Tag != null)
			{
				textBoxUser.Text = "";
				textBoxUser.Tag = null;
				textBoxUser.Font = new Font(textBoxUser.Font.FontFamily, textBoxUser.Font.Size, FontStyle.Regular, GraphicsUnit.Point, textBoxUser.Font.GdiCharSet);
				textBoxUser.ForeColor = SystemColors.WindowText;
			}
		}
	}
}
