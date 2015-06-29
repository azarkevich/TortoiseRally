using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using RallyToolsCore.Properties;
using RallyToolsCore.Review;
using TrackGearLibrary.Core;
using TrackGearLibrary.Core.MergeMessage;
using TrackGearLibrary.Rally;
using TrackGearLibrary.Svn;
using System.Threading.Tasks;
using UIControls.Core;

namespace TrackGearDesktop
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Logger.LogIt("Start args:\n{0}", string.Join("\n", args));

			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExe);

			AppDomain.CurrentDomain.UnhandledException += (s, e) => {
				Logger.LogIt("UnhandledException:");
				Logger.LogIt(e.ExceptionObject.ToString());
			};

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			Application.ThreadException += (s, e) => {
				Logger.LogIt("ThreadException:");
				Logger.LogIt(e.ToString());
			};

			var command = "rally";

			Dictionary<string, string> argdict = null;

			if(args.Length > 0)
			{
				argdict = args
					.Where(a => a.StartsWith("/") && a.Contains(":"))
					.Select(a => a.Substring(1))
					.Select(a => new { Key = a.Substring(0, a.IndexOf(':')).ToLowerInvariant(), Value = a.Substring(a.IndexOf(':') + 1) })
					.ToDictionary(arr => arr.Key, arr => arr.Value)
				;

				if(argdict.ContainsKey("command"))
					command = argdict["command"];
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (argdict != null && argdict.ContainsKey("version"))
			{
				UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeForceVersion);
				NewVersionControl.ForcedProductVersion = argdict["version"];
			}

			var resetSetings = false;
			if (argdict != null && argdict.ContainsKey("settings") && argdict["settings"] == "reset")
			{
				resetSetings = true;
				Settings.Default.Reset();
				Settings.Default.Save();
				UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeResetSettings);
			}

			try
			{
				Bootstrapper.Init(resetSetings);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Initialization error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			switch(command)
			{
				case "url":
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeCmdUrl);
					var urlcmd = args.FirstOrDefault(a => a.ToLowerInvariant().StartsWith("/urlcmd:"));
					if(urlcmd != null)
					{
						urlcmd = urlcmd.Substring("/urlcmd:".Length);

						var urlargs = urlcmd.Split(':');

						ProcessUrlCommand(urlargs);
					}
					break;

				case "help":
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeCmdHelp);
					ShowHelp();
					break;

				case "rally":
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeCmdRally);
					Application.Run(new RallyTool(false, null));
					break;

				case "daily-report":
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeCmdDailyReport);
					Application.Run(new DailyReport());
					break;

				case "merge-msg":
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeCmdMergeMessage);
					BuildMergeMsg(argdict);
					break;

				case "log":
					Debug.Assert(argdict != null);
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeCmdLog);
					if (!argdict.ContainsKey("path") && !argdict.ContainsKey("url"))
					{
						MessageBox.Show("Specify /path:<wc path> or /url:<branch url>");
						break;
					}
					new SvnLog(argdict.ContainsKey("path") ? argdict["path"] : argdict["url"]).ShowDialog();
					break;

				case "ex1":
					Debug.Assert(argdict != null);
					new ReviewersList().ShowDialog();
					break;
			}
		}

		static void BuildMergeMsg(IDictionary<string, string> args)
		{
			Debug.Assert(args != null);

			if (!args.ContainsKey("path") && !(args.ContainsKey("url") && args.ContainsKey("rev")))
			{
				MessageBox.Show("Specify /path:<wc path> or /url:<branch url> and /rev:<commit revision>");
				return;
			}

			var what = (args.ContainsKey("path")) ? args["path"] : args["url"] + "@" + args["rev"];

			var cts = new CancellationTokenSource();

			Task.Factory
				.StartNew(() => {

					if (args.ContainsKey("path"))
						return new MergeMessageComposer(cts.Token).ComposeForPath(args["path"]);

					return new MergeMessageComposer(cts.Token).ComposeForUrl(args["url"], long.Parse(args["rev"]));
				})
				.WaitDlg("Generation merge message for:\n" + what, 0, cts)
				.ContinueWith(t => {
					if (t.IsFaulted)
					{
						Debug.Assert(t.Exception != null);
						MessageBox.Show(t.Exception.InnerExceptions.First().ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						Clipboard.SetText(t.Result);
						MessageBox.Show("Merge message in clipboard", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled)
			;
		}

		static void ProcessUrlCommand(string[] urlargs)
		{
			if(urlargs.Length < 2)
			{
				ShowUrlHelp();
				Environment.Exit(-1);
			}

			var cmd = urlargs[1].ToLowerInvariant();
			urlargs = urlargs.Skip(2).ToArray();

			switch(cmd)
			{
				case "sign-defects":
					new SignDefects(urlargs).ShowDialog();
					break;
				case "vssdiff":
					UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.MainExeUrlCmdVssDiff);
					MessageBox.Show("vssdiff");
					break;
				default:
					ShowUrlHelp();
					Environment.Exit(-1);
					break;
			}
		}

		private static void ShowHelp()
		{
			MessageBox.Show(@"tgd.exe - open issues list (default command 'issues')

tgd.exe /command:<cmd>

execute command:

issue - default command. Show issues list.
sscommits - show dialog with last commits on SourceSafe (analyze ss_journal.txt).
help - this help.
revtools - same tools as for just commited issue, but for arbitrary revision and path.
ex1 - post commit tools example #1.
merge-msg - generate merge message. Params: /path:<wc> or /url:<url> + /rev:<revision>
log - log dialog. Params: /url:<url> or /path:<wc>
", "Rally Tools help");
		}

		static void ShowUrlHelp()
		{
			MessageBox.Show(@"tdgcmd url handler can't handle request.
Possible you are using old version of tgd? Try update first.
", "Rally Tools url protocol");
		}
	}
}
