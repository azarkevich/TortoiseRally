using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RallyToolsCore.Properties;
using SharpSvn;
using SharpSvn.Security;
using UIControls;
using UIControls.Core;

namespace TrackGearLibrary.Svn
{
	public partial class SvnLog : Form
	{
		readonly bool _autlStart;

		public SvnLog(string pathOrLog)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.SvnLog);
			
			InitializeComponent();

			colRevision.TextAlign = HorizontalAlignment.Right;

			if (!string.IsNullOrWhiteSpace(pathOrLog))
			{
				_autlStart = true;
				textBoxUrl.Text = pathOrLog;
				Settings.Default.LastSvnLogUrl = pathOrLog;
				Settings.Default.Save();
			}
			else
			{
				textBoxUrl.Text = Settings.Default.LastSvnLogUrl;
			}
		}

		// first load
		string _path;
		Uri _url;
		Uri _repoUri;

		SvnLogEventArgs _lastLoaded;

		static readonly Regex UrlScheme = new Regex("^https?://", RegexOptions.IgnoreCase);

		void buttonLoad_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.SvnLogLoad);

			_lastLoaded = null;
			buttonLoadNext.Enabled = true;
			listViewLog.Items.Clear();

			_path = null;
			_url = null;

			Settings.Default.LastSvnLogUrl = textBoxUrl.Text;
			Settings.Default.Save();

			if (UrlScheme.IsMatch(textBoxUrl.Text))
				_url = new Uri(textBoxUrl.Text);
			else if(Directory.Exists(textBoxUrl.Text) || File.Exists(textBoxUrl.Text))
				_path = textBoxUrl.Text;

			LoadRevisions();
		}

		void LoadRevisions()
		{
			int limit;

			Int32.TryParse(textBoxLoadBunch.Text, out limit);

			var cts = new CancellationTokenSource();

			Task.Factory
				.StartNew(() => {
					using (var client = new SvnClient())
					{
						client.Authentication.SslServerTrustHandlers += (sender, e) => { e.AcceptedFailures = SvnCertificateTrustFailures.MaskAllFailures; };
						client.Authentication.DefaultCredentials = CredentialCache.DefaultNetworkCredentials;

						SvnInfoEventArgs info;
						client.GetInfo(_url != null ? (SvnTarget)new SvnUriTarget(_url) : new SvnPathTarget(_path), out info);
						_repoUri = info.RepositoryRoot;

						var args = new SvnLogArgs { Limit = limit };

						var url = _url;

						if (_lastLoaded != null)
						{
							args.OperationalRevision = _lastLoaded.Revision - 1;
						}

						Collection<SvnLogEventArgs> entries;
						if (url != null)
							client.GetLog(url, args, out entries);
						else
							client.GetLog(_path, args, out entries);

						return entries;
					}
				}, cts.Token)
				.TrackProgress(loadingOperation, cts)
				.LockControls(textBoxUrl, buttonLoad, buttonLoadNext, textBoxLoadBunch)
				.ContinueWith(t => {
		
					if (t.IsFaulted || t.IsCanceled)
					{
						// if error occured - strat from scratch: can be load first N entries, can't continue
						_lastLoaded = null;
						buttonLoad.Enabled = true;
						buttonLoadNext.Enabled = false;
					}

					cts.Token.ThrowIfCancellationRequested();

					var entries = t.Result;

					if (entries.Count < limit)
					{
						// nothing to load next - all revisions loaded
						buttonLoadNext.Enabled = false;
						_lastLoaded = null;
					}
					else
					{
						_lastLoaded = entries.Last();
					}

					try
					{
						listViewLog.BeginUpdate();

						foreach (var entry in entries)
						{
							var lvi = new ListViewItem(new[] { entry.Revision.ToString(CultureInfo.InvariantCulture), entry.Author, entry.LogMessage })
							{
								Tag = entry
							};
							listViewLog.Items.Add(lvi);
						}
					}
					finally
					{
						listViewLog.EndUpdate();
					}

				}, TaskScheduler.FromCurrentSynchronizationContext())
				//.TrackProgress(loadingOperation)
				.WithDisposeWeak(cts)
			;
		}

		void textBoxUrl_TextChanged(object sender, EventArgs e)
		{
			buttonLoad.Enabled = true;
			buttonLoadNext.Enabled = true;
		}

		private void SvnLog_Load(object sender, EventArgs e)
		{
			if (_autlStart)
			{
				buttonLoad_Click(null, null);
			}
		}

		void buttonLoadNext_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.SvnLogLoadNext);

			if (_lastLoaded == null)
			{
				buttonLoad_Click(null, null);
				return;
			}

			LoadRevisions();
		}

		static bool IsFileModified(SvnChangeItem c)
		{
			return c.NodeKind == SvnNodeKind.File;
		}

		void generateReviewRequestToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.SvnLogGenerateReviewRequest);

			var entry = (listViewLog.FocusedItem.Tag as SvnLogEventArgs);
			if(entry == null)
				return;

			try
			{
				var paths = entry
					.ChangedPaths
					.Where(IsFileModified)
					.Select(cp => cp.Path)
					.ToArray()
				;

				new CommitedIssueTools(paths, entry.LogMessage, entry.Revision, _repoUri.OriginalString).ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
