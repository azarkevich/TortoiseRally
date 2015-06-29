using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using SharpSvn;
using TrackGearLibrary.Tools;

namespace TrackGearLibrary.Core.MergeMessage
{
	public class MergeMessageComposer
	{
		SvnClient _client;
		Uri _repoRoot;
		readonly Dictionary<long, RevLogEntry> _revLogEntries = new Dictionary<long, RevLogEntry>();
		string _requestedMergeMessageForBranch;

		readonly CancellationToken _cancellationToken;

		public MergeMessageComposer(CancellationToken ct)
		{
			_cancellationToken = ct;
		}

		public string ComposeForUrl(string branchUrl, long revision)
		{
			// get props
			using (_client = new SvnClient())
			{
				SvnInfoEventArgs info;
				_client.GetInfo(new SvnUriTarget(branchUrl, new SvnRevision(revision)), out info);
				_cancellationToken.ThrowIfCancellationRequested();
				_repoRoot = info.RepositoryRoot;
				_requestedMergeMessageForBranch = "/" + info.Path.Trim('/') + "/";

				var mergedRevisions = GetMergedRevisions(branchUrl, revision);

				LoadLogEntries(mergedRevisions);

				return BuildMessage(mergedRevisions);
			}
		}

		public string ComposeForPath(string wc)
		{
			// find root (where .svn reside)
			var wcRoot = SvnUtils.FindSvnWC(wc);
			if (wcRoot == null)
				throw new ApplicationException("Can't find working copy root for " + wc);

			using (_client = new SvnClient())
			{
				var wcTarg = new SvnPathTarget(wcRoot);

				SvnInfoEventArgs info;
				_client.GetInfo(wcTarg, out info);
				_cancellationToken.ThrowIfCancellationRequested();
				_repoRoot = info.RepositoryRoot;
				_requestedMergeMessageForBranch = "/" + info.Path.Trim('/') + "/";

				SvnTargetPropertyCollection mergeInfoPre;
				if (!_client.GetProperty(wcTarg, "svn:mergeinfo", new SvnGetPropertyArgs { Revision = SvnRevision.Base }, out mergeInfoPre))
					throw new ApplicationException("Error in GetProperty");

				string mergeInfoNew;
				if (!_client.GetProperty(wcTarg, "svn:mergeinfo", out mergeInfoNew))
					throw new ApplicationException("Error in GetProperty");

				var baseMergeInfo = new Dictionary<string, RevRangeSet>();

				if (mergeInfoPre != null && mergeInfoPre.Count != 0)
				{
					baseMergeInfo = ParseMegeinfoLines(mergeInfoPre[0].StringValue);
				}

				var currentMergeInfo = ParseMegeinfoLines(mergeInfoNew);

				var newRevs = Subtract(currentMergeInfo, baseMergeInfo);

				LoadLogEntries(newRevs);

				return BuildMessage(newRevs);
			}
		}

		Dictionary<string, RevRangeSet> GetMergedRevisions(string branchUrl, long revision)
		{
			var targ = new SvnUriTarget(branchUrl, new SvnRevision(revision));
			var targPrev = new SvnUriTarget(branchUrl, new SvnRevision(revision - 1));

			string mergeInfoA;
			try
			{
				if (!_client.GetProperty(targPrev, "svn:mergeinfo", out mergeInfoA))
					throw new ApplicationException("Error in GetProperty");
				_cancellationToken.ThrowIfCancellationRequested();
			}
			catch (SvnEntryNotFoundException)
			{
				// no base version
				return new Dictionary<string, RevRangeSet>();
			}

			string mergeInfoB;
			if (!_client.GetProperty(targ, "svn:mergeinfo", out mergeInfoB))
				throw new ApplicationException("Error in GetProperty");
			_cancellationToken.ThrowIfCancellationRequested();

			var baseMergeInfo = ParseMegeinfoLines(mergeInfoA ?? "");
			var newMergeInfo = ParseMegeinfoLines(mergeInfoB ?? "");

			var ret = Subtract(newMergeInfo, baseMergeInfo);

			//var dump = Dump(ret);

			return ret;
		}

		string BuildMessage(Dictionary<string, RevRangeSet> newRevs)
		{
			var sb = new StringBuilder();

			foreach (var kvp in newRevs)
			{
				sb.AppendFormat("Merged revision(s) {0} from {1}:\n", kvp.Value, kvp.Key);
			}

			sb.AppendLine();

			foreach (var rev in _revLogEntries.Keys.OrderBy(k => k))
			{
				var log = _revLogEntries[rev];
				if (log == null)
					continue;

				var msg = string.Format("r{0} by {1}\n{2}\n---------------------", rev, log.Author, log.Message.Trim());
				sb.AppendLine(msg);
			}

			return sb.ToString();
		}

		void LoadLogEntries(Dictionary<string, RevRangeSet> branchRevs)
		{
			foreach (var kvp in branchRevs)
			{
				var branchUri = new Uri(_repoRoot.ToString().TrimEnd('/') + kvp.Key);
				LoadLogEntries(branchUri, kvp.Value);
			}
		}

		static bool IsPossibleMergeRevision(SvnChangeItem c)
		{
			return c.NodeKind == SvnNodeKind.Directory && c.Action == SvnChangeAction.Modify;
		}

		void LoadLogEntries(Uri branchUri, RevRangeSet revRangesSet)
		{
			var min = revRangesSet.Ranges.First().RangeStart;
			var max = revRangesSet.Ranges.Last().RangeEnd;

			var args = new SvnLogArgs(new SvnRevisionRange(min, max)) {
				RetrieveChangedPaths = true,
				OperationalRevision = min
			};

			Collection<SvnLogEventArgs> logEntries;
			_client.GetLog(branchUri, args, out logEntries);
			_cancellationToken.ThrowIfCancellationRequested();

			foreach (var logEntry in logEntries)
			{
				if (!revRangesSet.ContainsRevision(logEntry.Revision))
					continue;

				if (_revLogEntries.ContainsKey(logEntry.Revision))
					continue;

				// mark as in progress
				_revLogEntries[logEntry.Revision] = null;

				var changedDirs = logEntry.ChangedPaths.Where(IsPossibleMergeRevision).ToArray();

				var mergeRevision = false;
				foreach (var cd in changedDirs)
				{
					var changedUri = new Uri(_repoRoot.ToString().TrimEnd('/') + cd.Path);
					var merged = GetMergedRevisions(changedUri.ToString(), logEntry.Revision);
					if (merged.Count > 0)
					{
						mergeRevision = true;

						// exclude merged revisions from branch which was requested. to avoid merged messages from
						// trunk < reintegrate feature < sync trunc
						// sync trunk messages shall be droped
						while (true)
						{
							var removed = false;
							foreach (var kvp in merged)
							{
								if ((kvp.Key.TrimEnd('/') + "/").StartsWith(_requestedMergeMessageForBranch))
								{
									merged.Remove(kvp.Key);
									removed = true;
									break;
								}
							}

							if (!removed)
								break;
						}

						if (merged.Count > 0)
							LoadLogEntries(merged);
					}
				}

				// not store merge revisions message
				if (mergeRevision)
					continue;

				// check if it is agan merge revision
				_revLogEntries[logEntry.Revision] = new RevLogEntry {
					Author = logEntry.Author,
					Message = logEntry.LogMessage,
					Revision = logEntry.Revision
				};
			}
		}

		static string Dump(Dictionary<string, RevRangeSet> newRevs)
		{
			var sb = new StringBuilder();

			foreach (var kvp in newRevs)
			{
				var ranges = string.Join(",", kvp.Value.Ranges.Select(r => r.ToString()).ToArray());
				sb.AppendFormat("{0}:{1}\n", kvp.Key, ranges);
			}

			return sb.ToString();
		}

		static Dictionary<string, RevRangeSet> Subtract(Dictionary<string, RevRangeSet> from, Dictionary<string, RevRangeSet> what)
		{
			var ret = new Dictionary<string, RevRangeSet>();
			foreach (var kvp in from)
			{
				if (!what.ContainsKey(kvp.Key))
				{
					// completly new branch. add all new ranges
					ret.Add(kvp.Key, kvp.Value);
				}
				else
				{
					var subtracted = kvp.Value.Subtract(what[kvp.Key]);
					if (subtracted.Ranges.Count() != 0)
						ret.Add(kvp.Key, subtracted);
				}
			}

			return ret;
		}

		static Dictionary<string, RevRangeSet> ParseMegeinfoLines(string text)
		{
			var lines = text.Replace('\r', '\n').Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			var dict = new Dictionary<string, RevRangeSet>();

			foreach (var line in lines)
			{
				var parts = line.Split(':');
				if(parts.Length != 2)
					throw new ApplicationException("Invalid (?) merge info line: " + line);

				var branch = parts[0];

				var rangesList = new List<RevRange>();

				foreach (var range in parts[1].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(r => r.TrimEnd('*')))
				{
					var rangePair = range.Split('-');
					if (rangePair.Length == 1)
						rangesList.Add(new RevRange { RangeStart = Int32.Parse(rangePair[0]), RangeEnd = Int32.Parse(rangePair[0]) });
					else if(rangePair.Length == 2)
						rangesList.Add(new RevRange { RangeStart = Int32.Parse(rangePair[0]), RangeEnd = Int32.Parse(rangePair[1]) });
					else
						throw new ApplicationException(string.Format("Invalid (?) range '{0}' in merge info line: '{1}'", range, line));
				}

				dict[branch] = new RevRangeSet(rangesList).Normalize();
			}

			return dict;
		}
	}
}
