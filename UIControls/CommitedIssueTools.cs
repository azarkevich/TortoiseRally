using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Web;
using Rally.RestApi;
using RallyToolsCore.Properties;
using SharpSvn;
using TrackGearLibrary.Core;
using TrackGearLibrary.Rally;
using UIControls;
using UIControls.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

// ReSharper disable InconsistentNaming
namespace TrackGearLibrary
{
	public partial class CommitedIssueTools : Form
	{
		readonly ICollection<string> _changedItems;
		readonly long _revision;
		readonly TicketItem[] _issues;
		readonly string _repoUrl;
		readonly string _baseDir;

		readonly Regex _rxIssue = new Regex(@"#(?<project>[a-zA-Z0-9_]+)\.(?<report>\d+):\s*(?<summary>.*)$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
		readonly Regex _rxRallyIssue = new Regex(@"(?<project>DE|TA|US)(?<report>\d+)(?:\s*:\s*(?<summary>.+)(?:\n|$))?", RegexOptions.Multiline);

		Task<string> _regressionBuild;

		string _fdpFilePath;
		readonly Task<object> _resolveRallyUrlsTask;

		public static void ShowCommitedIssueTools(IntPtr hParentWnd, string commonRoot, string[] pathList, string logMessage, int revision)
		{
			if (!Settings.Default.EnablePostCommitTools)
				return;

			var repoUrl = "";
			var repoRelPathList = pathList;
			try
			{
				SvnInfoEventArgs info;
				using (var client = new SvnClient())
				{
					client.GetInfo(new SvnPathTarget(commonRoot), out info);
					repoUrl = info.RepositoryRoot.OriginalString;
				}

				// calculate paths relative to repository:
				var commonRootRepoRelPath = info.Uri.OriginalString.Substring(repoUrl.Length).Trim('/');
				repoRelPathList = pathList
					.Select(p =>
					{
						var relToCommon = p.Substring(commonRoot.Length).Replace('\\', '/').TrimStart('/');
						return commonRootRepoRelPath + "/" + relToCommon;
					})
					.ToArray()
				;
			}
			catch (Exception ex)
			{
				MessageBoxEx.ShowWarning("Error while detecting repository url: " + ex.Message);
			}

			new CommitedIssueTools(repoRelPathList, logMessage, revision, repoUrl) {
				Parent = Control.FromHandle(hParentWnd),
				StartPosition = FormStartPosition.CenterParent
			}.ShowDialog();
		}

		public CommitedIssueTools(ICollection<string> rawPathList, string logMessage, long revision, string repoUrl)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CommitTool);

			Debug.Assert(rawPathList.Count > 0);

			_repoUrl = repoUrl;

			_revision = revision;

			_baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			_changedItems = rawPathList;

			// guess cause
			var tgIssues = _rxIssue
				.Matches(logMessage)
				.Cast<Match>()
				.Select(m => new TicketItem(Int32.Parse(m.Groups["report"].Value.Trim()), m.Groups["project"].Value.Trim(), m.Groups["summary"].Value.Trim()))
			;

			var rallyIssues = _rxRallyIssue
				.Matches(logMessage)
				.Cast<Match>()
				.Select(m => new TicketItem(Int32.Parse(m.Groups["report"].Value.Trim()), m.Groups["project"].Value.Trim(), m.Groups["summary"].Value.Trim()) { RallyItem = true, RallyFormattedID =  m.Groups["project"].Value.Trim() + m.Groups["report"].Value.Trim() })
				.ToArray()
			;

			InitializeComponent();

			if (rallyIssues.Length > 0)
			{
				if(string.IsNullOrWhiteSpace(Settings.Default.RallyUser))
				{
					new RallySettings().ShowDialog();
				}

				if(!string.IsNullOrWhiteSpace(Settings.Default.RallyUser))
				{
					_resolveRallyUrlsTask = Task.Factory
						.StartNew(() => { ResolveRallyDefects(rallyIssues); return (object)null; })
					;

					// set 'Completed' text
					bgOperationProgress.Text = "Rally URLs resolved";
					_resolveRallyUrlsTask
						.TrackProgress(bgOperationProgress, "Detecting Rally URLs for items")
					;
				}
			}

			_issues = tgIssues.Concat(rallyIssues).ToArray();

			linkLabelRegressBuild.LinkArea = new LinkArea(0, 0);
		}

		static void ResolveRallyDefects(TicketItem[] rallyItems)
		{
			var restApi = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword
				//, proxy: new WebProxy("localhost:8888", false)
			);

			var request = new Request("artifact");
			request.Fetch = new List<string> { "FormattedID", "ObjectID", "Project", "Name" };

			// build query for many items
			var queries = rallyItems.Select(it => new Query("FormattedID", Query.Operator.Equals, it.RallyFormattedID)).ToArray();

			var query = queries[0];
			for (var i = 1; i < queries.Length; i++)
			{
				query = query.Or(queries[i]);
			}

			request.Query = query;

			// perfrom query
			var resp = restApi.Query(request);

			foreach (var result in resp.Results)
			{
				var art = new Artifact(result);

				var fid = art.FormattedID;
				var ticket = rallyItems.FirstOrDefault(t => StringComparer.OrdinalIgnoreCase.Compare(t.RallyFormattedID, fid) == 0);
				if (ticket != null)
				{
					// fill summary if absent
					if (string.IsNullOrWhiteSpace(ticket.Summary) && art.Name != null)
						ticket.Summary = art.Name;

					// build URL
					var pid = 0L;
					if (!art.Project.IsNullArtifact)
					{
						pid = art.Project.ObjectID;
					}
					var oid = art.ObjectID;

					var type = (string)result["_type"];

					ticket.RallyUrl = string.Format("https://rally1.rallydev.com/#/{0}/detail/{1}/{2}", pid, type.ToLowerInvariant(), oid);
					ticket.RallyArtifact = art;
				}
			}
		}

		void buttonGenerateFDP_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CommitToolGenerateFDP, (sender == buttonGenFdpToFile));

			if (sender == buttonGenFdpToFile && _fdpFilePath != null)
			{
				Process.Start(_fdpFilePath);
				return;
			}

			try
			{
				var sb = new StringBuilder();

				sb.Append(File.ReadAllText(Path.Combine(_baseDir, "fdp-template.txt")));

				var nextBuild = CancellableWait.Wait("Next build...", _regressionBuild);
				if (nextBuild.Status == TaskStatus.RanToCompletion)
					sb.Replace("$$NEXTBUILD$$", nextBuild.Result);
				else if (nextBuild.Status == TaskStatus.Faulted)
					sb.Replace("$$NEXTBUILD$$", "ERROR");
				else if (nextBuild.Status == TaskStatus.Canceled)
					sb.Replace("$$NEXTBUILD$$", "CANCELLED");

				// guess cause
				var cause = _issues
					.Select(i => i.Summary.Trim())
					.Where(s => !string.IsNullOrEmpty(s))
					.Aggregate(new StringBuilder(), (agg, s) => agg.AppendLine("\t" + s))
					.ToString()
				;

				sb.Replace("$$CAUSE$$", "	TBD:\r\n" + cause);

				// detect touched modules
				var dt = DateTime.Now.ToString("yyyy\\/MM\\/dd");
				var modules = GuessModules(_changedItems.Where(p => p.Length > 1 && p[1] == Path.VolumeSeparatorChar))
					.DefaultIfEmpty("UnkModule")
					.Select(m => m + " " + dt)
					.Aggregate(new StringBuilder(), (agg, s) => agg.AppendLine("\t" + s))
					.ToString()
				;

				sb.Replace("$$MODULES$$", modules);

				// detect changed sources
				var paths = _changedItems
					.Aggregate(new StringBuilder(), (agg, s) => agg.AppendLine("\t" + s))
					.ToString()
				;

				paths += "	in revision " + _repoUrl.TrimEnd('/') + "@" + _revision;

				sb.Replace("$$PATHS$$", paths);

				// reviewer
				var reviewer = "TBD";
				var r = comboBoxReviewer.SelectedItem as MailAddress;
				if (r != null)
				{
					reviewer = r.DisplayName;
					if(string.IsNullOrWhiteSpace(reviewer))
						reviewer = r.User;
				}
				else if (!string.IsNullOrWhiteSpace(comboBoxReviewer.Text))
				{
					reviewer = comboBoxReviewer.Text;
				}

				sb.Replace("$$REVIEWER$$", reviewer);

				if (sender == buttonGenFDP)
				{
					Clipboard.SetText(sb.ToString());

					buttonGenFDP.BackColor = Color.LightGreen;

					Observable.Timer(TimeSpan.FromMilliseconds(500))
						.Subscribe(_ => buttonGenFDP.BackColor = Color.White)
					;
				}
				else if (sender == buttonGenFdpToFile)
				{
					var path = Path.GetTempFileName();
					_fdpFilePath = path + ".fdp.txt";
					File.Move(path, _fdpFilePath);

					File.WriteAllText(_fdpFilePath, sb.ToString());

					Process.Start(_fdpFilePath);

					if(_issues.Length == 1)
					{
						buttonPostFDP.Enabled = true;
					}
				}
			}
			catch(Exception ex)
			{
				Logger.LogIt(ex.ToString());
			}
		}

		static IEnumerable<string> GuessModules(IEnumerable<string> paths)
		{
			return paths
				.Select(FindModule)
				.Where(m => m != null)
				.Distinct()
				.ToArray()
			;
		}

		static string FindModule(string path)
		{
			if (Directory.Exists(path) && Directory.GetFiles(path, "*.*proj").Length > 0)
			{
				return Path.GetFileNameWithoutExtension(Directory.GetFiles(path, "*.*proj")[0]);
			}

			var dir = Path.GetDirectoryName(path);

			if (dir == path)
				return null;

			return FindModule(dir);
		}

		string GetHtmlReviewRequestBody()
		{
			var sbBody = new StringBuilder(File.ReadAllText(Path.Combine(_baseDir, "review-email-template.html")));

			sbBody.Replace("$$BRURL$$", _repoUrl);

			string ver;
			try
			{
				ver = ((AssemblyInformationalVersionAttribute)Assembly
					.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0])
					.InformationalVersion
				;
			}
			catch
			{
				ver = NewVersionControl.ProductVersion;
			}

			sbBody.Replace("$$TGL-VERSION$$", ver);
			sbBody.Replace("$$REV$$", _revision.ToString(CultureInfo.InvariantCulture));
			sbBody.Replace("$$PREV_REV$$", (_revision - 1).ToString(CultureInfo.InvariantCulture));

			Func<TicketItem, string> formatTicket = t => {
				if (!t.RallyItem)
				{
					return string.Format("#<a href='http://issues/view?issue={0}.{1}'>{0}.{1}</a>: {2}<br/>",
						HttpUtility.HtmlEncode(t.Project),
						t.Number,
						HttpUtility.HtmlEncode(t.Summary)
					);
				}

				var sb = new StringBuilder();

				var rallyUrl = t.RallyUrl;

				// append 'search URL'
				sb.AppendFormat("<a href='https://rally1.rallydev.com/#/0/search?keywords={0}'>{0}</a>", t.RallyFormattedID);

				if (rallyUrl != null)
				{
					sb.AppendFormat("(<a href='{0}/discussion'>discussion</a>)", rallyUrl);
				}

				if (!string.IsNullOrWhiteSpace(t.Summary))
					sb.Append(": " + t.Summary);

				sb.AppendLine("<br />");

				return sb.ToString();
			};

			var issues = _issues
				.Aggregate(new StringBuilder(), (agg, t) => agg.AppendLine(formatTicket(t)))
				.ToString()
			;

			sbBody.Replace("$$ISSUES$$", issues);

			var paths = _changedItems
				.Aggregate(new StringBuilder(), (agg, s) => agg.AppendLine(HttpUtility.HtmlEncode(s) + "<br/>"))
				.ToString()
			;

			sbBody.Replace("$$PATHS$$", paths);

			var rallyDefects = string.Join(":", _issues.Where(i => i.RallyItem).Select(i => i.RallyFormattedID));

			if(!string.IsNullOrWhiteSpace(rallyDefects))
				sbBody.Replace("$$TGD-SIGN-ISSUES-URL$$", string.Format("Or with <a href='tgdcmd:sign-defects:{0}'>tgd</a>", rallyDefects));
			else
				sbBody.Replace("$$TGD-SIGN-ISSUES-URL$$", "");

			return sbBody.ToString();
		}

		string GetReviewRequestSubject()
		{
			var issuesShort = _issues
				.Select(iss => {
					if(!iss.RallyItem)
						return string.Format("#{0}.{1}", iss.Project, iss.Number);

					return iss.Project + iss.Number;
				})
				.ToArray()
			;

			return string.Format("Please review issues {0} fixed in in revision {1}", string.Join(", ", issuesShort), _revision);
		}

		void buttonSendForReview_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CommitToolComposeReviewRequestEMail);

			var subject = GetReviewRequestSubject();

			var body = GetHtmlReviewRequestBody();

			var app = new Outlook.Application();

			Outlook.MailItem msg;
			if (!string.IsNullOrWhiteSpace(Settings.Default.ReviewRequestTemplate))
			{
				msg = (Outlook.MailItem)app.CreateItemFromTemplate(Settings.Default.ReviewRequestTemplate);

				// make subject
				if(string.IsNullOrWhiteSpace(msg.Subject))
					msg.Subject = subject;
				else if (msg.Subject.Contains("$$SUBJECT$$"))
					msg.Subject = msg.Subject.Replace("$$SUBJECT$$", subject);
				else if (msg.Subject.Contains("$$NOSUBJECT$$"))
					msg.Subject = msg.Subject.Replace("$$NOSUBJECT$$", "");
				else
					msg.Subject = msg.Subject + " " + subject;

				msg.HTMLBody = BuildMailBody(body, msg.HTMLBody);
			}
			else
			{
				msg = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
				msg.Subject = subject;
				msg.HTMLBody = BuildMailBody(body);
			}

			var r = comboBoxReviewer.SelectedItem as MailAddress;
			if(r != null)
				msg.To = r.Address;

			msg.Display();
		}

		const string DefaultEmailTemplate = @"
<HTML>
Hello,
<br/>
<br/>
$$BODY$$
<br/>
<br/>
</HTML>
";

		static string BuildMailBody(string bodyText, string template = DefaultEmailTemplate)
		{
			if (template.Contains("$$BODY$$"))
				return template.Replace("$$BODY$$", bodyText);

			return bodyText;
		}

		void buttonComposeReviewRequestHtml_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CommitToolComposeReviewRequestHtml);

			var subject = GetReviewRequestSubject();

			var body = GetHtmlReviewRequestBody();

			var path = Path.GetTempFileName();
			var newPath = path + ".rr.txt";
			File.Move(path, newPath);

			File.WriteAllText(newPath, subject + "\r\n\r\n");
			File.AppendAllText(newPath, "===\r\n\r\n");
			File.AppendAllText(newPath, body);

			Process.Start(newPath);
		}

		void buttonOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		void CommitedIssueTools_Load(object sender, EventArgs e)
		{
			// fill reviewers
			var rwFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "conf\\reviewers.txt");
			if(File.Exists(rwFile))
			{
				var reviewers = File.ReadAllLines(rwFile)
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.Select(l => new MailAddress(l))
					.ToArray()
				;

				comboBoxReviewer.Items.AddRange(reviewers);
			}

			if(comboBoxReviewer.Items.Count == 0)
			{
				comboBoxReviewer.Items.Add("Fill 'conf\\reviewers.txt'");
			}

			var ts = new TaskCompletionSource<string>();
			_regressionBuild = ts.Task;

			var nextBuildUrl = Settings.Default.NextBuildUrlTemplate.Replace("{branch}", "trunk");

			if(!string.IsNullOrWhiteSpace(nextBuildUrl))
			{

				var wc = new WebClient { UseDefaultCredentials = true };

				wc.DownloadStringAsync(new Uri(nextBuildUrl));
				wc.DownloadStringCompleted += (o, args) =>
				{

					var err = args.Error;
					var res = args.Error == null ? args.Result : null;

					Action a = () =>
					{

						if (err != null)
						{
							linkLabelRegressBuild.Text = "Next build(s): Error: " + err.Message;
							ts.TrySetException(err);
							return;
						}

						int build;

						if (Int32.TryParse(res, out build) && build > 0)
						{
							build++;

							var sbuild = build.ToString(CultureInfo.InvariantCulture);

							linkLabelRegressBuild.Tag = build;
							linkLabelRegressBuild.Text = "Next build(s): " + sbuild;
							linkLabelRegressBuild.LinkArea = new LinkArea("Next build(s): ".Length, linkLabelRegressBuild.Text.Length - "Next build(s): ".Length);
							linkLabelRegressBuild.Click += (_, __) => Clipboard.SetText(sbuild);

							ts.TrySetResult(sbuild);
						}
						else
						{
							ts.TrySetResult("No information");
							linkLabelRegressBuild.Text = "Next build(s): Result: " + res;
						}
					};

					if (InvokeRequired)
						Invoke(a);
					else
						a();
				};
			}
			else
			{
				ts.SetCanceled();
			}

			Task.Factory.StartNew(() =>
			{

				var waiters = new[] { "", ".", "..", "...", "...", "...." };
				var waiterIdx = 0;

				while (true)
				{
					linkLabelRegressBuild.Invoke((Action)delegate
					{
						if (linkLabelRegressBuild.Handle != IntPtr.Zero && !_regressionBuild.IsCompleted)
							linkLabelRegressBuild.Text = "Guess build to regress: " + waiters[waiterIdx++ % waiters.Length];
					});

					if (_regressionBuild.IsCompleted)
						break;

					Thread.Sleep(200);
				}
			});
		}

		void comboBoxReviewer_SelectedIndexChanged(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CommitToolReviewerChanged);

			// reshuffle
			var r = comboBoxReviewer.SelectedItem as MailAddress;
			if(r == null)
				return;

			var sb = new StringBuilder();
			sb.AppendLine(r.ToString());

			foreach (var item in comboBoxReviewer.Items)
			{
				var address = item as MailAddress;
				if(address == null || ReferenceEquals(item, r))
					continue;

				sb.AppendLine(address.ToString());
			}

			var rwFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "conf\\reviewers.txt");
			File.WriteAllText(rwFile, sb.ToString());
		}

		void buttonPostFDP_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CommitToolPostFDP);

			var t = CancellableWait.Wait("Resolve URL...", _resolveRallyUrlsTask);
			if(t.IsFaulted)
			{
				MessageBoxEx.ShowError("Resolve URL failed");
				return;
			}

			if (t.IsCanceled)
				return;

			try
			{
				buttonPostFDP.Enabled = false;

				UseWaitCursor = true;

				var restApi = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword
					//, proxy: new WebProxy("localhost:8888", false)
				);

				var textLines = File.ReadAllText(_fdpFilePath)
					.Split('\n')
					.Select(l => l.Replace("\r", ""))
				;

				var text = string.Join("<br/>", textLines.Select(HttpUtility.HtmlEncode).Select(l => l.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")));

				var art = _issues[0].RallyArtifact;

				var toCreate = new DynamicJsonObject();
				toCreate["Text"] = text;
				toCreate["Artifact"] = art.Reference;
				var createResult = restApi.Create("ConversationPost", toCreate);

				if (!createResult.Success)
				{
					MessageBoxEx.ShowError(string.Join("\n", createResult.Errors.DefaultIfEmpty("Unexpected")));
					return;
				}

				if (MessageBox.Show("Discussion post added.\nOpen discussion in browser?", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
					Process.Start(_issues[0].RallyUrl + "/discussion");
			}
			finally
			{
				buttonPostFDP.Enabled = true;
				UseWaitCursor = false;
			}
		}
	}
}
