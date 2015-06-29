using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UIControls.Core
{
	public static class UsageMetrics
	{
		readonly static object Lock = new object();

		// ReSharper disable InconsistentNaming
		public enum UsageKind
		{
			BugtrackInit,
			BugtrackMenuShiftKey,
			BugtrackGenerateMergeMessage,
			BugtrackSelectTGIssues,
			BugtrackCommitTools,
			BugtrackSelectRallyIssues,
			MainExe,
			MainExeForceVersion,
			MainExeResetSettings,
			MainExeCmdUrl,
			MainExeCmdHelp,
			MainExeCmdRally,
			MainExeCmdDailyReport,
			MainExeCmdSSCommits,
			MainExeCmdMergeMessage,
			MainExeUrlCmdVssDiff,
			MainExeCmdLog,
			TGIssues,
			TGIssuesOpenIssueUrl,
			TGIssuesCopyIssue,
			TGIssuesAdvancedFileter,
			TGIssuesOpenBBSite,
			TGIssuesVssBlame,
			TGIssuesSvnLog,
			TGIssuesAdvancedFilter,
			DailyReport,
			DailyReportClickLoad,
			DailyReportClickGenerate,
			DailyReportCopy,
			DailyReportChangePreset,
			RallySettings,
			RallyTool,
			RallyToolSettings,
			RallyToolOK,
			RallyToolItemDblClick,
			RallyToolApplyQuery,
			RallyToolSSCommits,
			RallyToolStartBB,
			RallyToolStartVssBlame,
			RallyToolStartSvnLog,
			RallyToolStartDailyReport,
			SvnLog,
			SvnLogLoad,
			SvnLogLoadNext,
			SvnLogGenerateReviewRequest,
			SSCommits,
			SSCommitsApply,
			SSCommitsGenerateFDP,
			SSCommitsComposeReviewRequest,
			SSCommitsLoad,
			SSCommitsBlame,
			SSCommitsUDiff,
			SSCommitsCreatePatch,
			SSCommitsCSV2Clip,
			PatchQueueFrom,
			CancellableWaitCancelled,
			CommitTool,
			CommitToolGenerateFDP,
			CommitToolComposeReviewRequestEMail,
			CommitToolComposeReviewRequestHtml,
			CommitToolReviewerChanged,
			TGIssue,
			UnpackTo,
			BGOperationCancelled,
			RallyToolContextMenuRefresh,
			RallyToolContextMenuCopy,
			RallyToolContextMenuOpenOnRally,
			RallyToolIterationFilterPrev,
			RallyToolIterationFilterNext,
			RallyToolShowHideDefectsSubtasks,
			RallyToolShowHideNoiseTasks,
			CommitToolPostFDP
		}

		public static string ProductVersion;

		static UsageMetrics()
		{
			ProductVersion = Assembly.GetAssembly(typeof(UsageMetrics)).GetName().Version.ToString();
		}

		public static void IncrementUsage(UsageKind usage, params object[] values)
		{
			try
			{
				var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.AppDataDirName);
				Directory.CreateDirectory(dir);

				var usageLine = string.Format("{0}	{1}	{2}	{3}\n", DateTimeOffset.Now.Ticks, ProductVersion, usage, string.Join("|", values.Select(v => v.ToString())));
				var path = Path.Combine(dir, "usage-metrics.txt");

				lock (Lock)
				{
					if (File.Exists(path))
						File.AppendAllText(path, usageLine);
					else
						File.WriteAllText(path, usageLine);
				}
			}
			catch
			{
			}
		}
	}
}
