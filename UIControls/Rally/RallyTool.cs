using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Rally.RestApi;
using RallyToolsCore.Properties;
using TrackGearLibrary.Core;
using TrackGearLibrary.Rally.Columns;
using TrackGearLibrary.Svn;
using UIControls.Core;

namespace TrackGearLibrary.Rally
{
	public partial class RallyTool : Form
	{
		const string CurrentIterationMarker = "E39F6C98-C157-4E11-9895-A161BD8E2A89";

		public Artifact[] SelectedWorkItems;

		readonly List<VirtualListItemArtifact> _filteredIssues = new List<VirtualListItemArtifact>();
		readonly List<VirtualListItemArtifact> _allIssues = new List<VirtualListItemArtifact>();

		readonly ColumnsCollection _columns;
		readonly string _commonRoot;
		readonly bool _selectIssuesMode;

		class LoadArtifactsBgOperation
		{
			public Task<List<Artifact>> Task;
			public CancellationTokenSource Cts;
		}

		LoadArtifactsBgOperation _firstArtifactsLoad;

		public static Color ChangedFilter = Color.Yellow;
		public static Color AppliedFilter = Color.LightGreen;
		public static Color EmptyFilter = Color.White;

		public readonly string _vssPowerToolsPath;

		public RallyTool(bool selectIssuesMode, string commonRoot)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyTool);

			_selectIssuesMode = selectIssuesMode;
			_commonRoot = commonRoot;

			InitializeComponent();

			Observable
				.FromEventPattern<EventHandler, EventArgs>(h => textBoxNameFilter.TextChanged += h, h => textBoxNameFilter.TextChanged -= h)
				.Do(_ => textBoxNameFilter.BackColor = ChangedFilter)
				.Throttle(TimeSpan.FromMilliseconds(500))
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(_ => UpdateFilteredArtifactsList())
			;

			var ver = new NewVersionControl();
			ver.Name = "VersionChecker";
			ver.Anchor = versionPlaceholder.Anchor;
			ver.Location = versionPlaceholder.Location;
			ver.Size = versionPlaceholder.Size;

			Controls.Add(ver);
			Controls.Remove(versionPlaceholder);

			_columns = InitColumns();

			// try find external tools
			try
			{
				using (var rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\VssPowerTools.exe", false))
				{
					_vssPowerToolsPath = (string)rk.GetValue(null);
				}
			}
			catch (Exception ex)
			{
				_vssPowerToolsPath = null;
				Logger.LogIt("Error: {0}", ex);
			}

			if(_vssPowerToolsPath == null)
			{
				sourceSafeCommitsToolStripMenuItem.Text += " (not installed)";
			}
		}

		void RallyTool_Load(object sender, EventArgs e)
		{
			Text = Text + " (ver. " + NewVersionControl.ProductVersion + ")";

			if(string.IsNullOrWhiteSpace(Settings.Default.RallyUser))
			{
				if (new RallySettings().ShowDialog() == DialogResult.OK)
				{
					// force refresh query
					Settings.Default.RallyQuery = null;
					EnsureQueryValid();
				}
			}

			// start loading artifacts on every form opening.
			// sometitme it will be in bg, sometime in fg
			_firstArtifactsLoad = StartLoadArtifacts();

			var lastUpdate = DateTimeOffset.MinValue;
			var artifacts = LoadCachedArtifacts(ref lastUpdate);

			if (artifacts != null)
			{
				SetArtifactsList(artifacts, lastUpdate);

				// and force update list if fingerprints does not match
				_firstArtifactsLoad
					.Task
					.ContinueWith(t => {
							var freshArtifacts = t.Result;
							if(CalcFingerprint(_filteredIssues.Select(i => i.WorkItem)) != CalcFingerprint(freshArtifacts))
							{
								SetArtifactsList(freshArtifacts, DateTimeOffset.Now);
							}
						},
						_firstArtifactsLoad.Cts.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext()
					)
				;
			}
			else
			{
				// force open 'wait for refresh' dialog
				RefreshArtifactsList();
			}
		}

		void EnsureQueryValid()
		{
			if (string.IsNullOrWhiteSpace(Settings.Default.RallyQuery))
			{
				var userName = Settings.Default.RallyUser;

				try
				{
					var rr = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword);

					var currentUser = rr.GetCurrentUser("UserName");

					userName = (string)currentUser["UserName"];
				}
				catch
				{
				}

				var ownerClause = string.Format("(Owner.Name = \"{0}\")", userName);

				Settings.Default.RallyQuery = string.Format("({0} and ((State < \"Completed\") or (State < \"Closed\")))", ownerClause);

				textBoxQuery.Text = Settings.Default.RallyQuery;

				Settings.Default.Save();
			}
		}

		void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolSettings);
			if (new RallySettings().ShowDialog() == DialogResult.OK)
			{
				// regenerate query
				Settings.Default.RallyQuery = null;
				EnsureQueryValid();
				RefreshArtifactsList();
			}
		}

		static string CalcFingerprint(IEnumerable<Artifact> artifacts)
		{
			return string.Join(";", artifacts.Select(a => string.Format("{0}x{1}", a.ObjectID, a.Version)));
		}

		IDisposable LockControlsForRefresh()
		{
			Action<bool> enableControls = enable =>
			{
				listViewIssues.Enabled = enable;
				textBoxQuery.Enabled = enable;
				buttonApplyQuery.Enabled = enable;
				buttonOK.Enabled = enable;
			};

			enableControls(false);

			return Disposable.Create(() => enableControls(true));
		}

		void RefreshArtifactsList()
		{
			try
			{
				EnsureQueryValid();

				var refreshOperation = StartOrPickupLoadArtifacts();

				var lockControls = LockControlsForRefresh();

				backgroundRefreshOperation
					.TrackWithCancellation(refreshOperation.Task, refreshOperation.Cts)
					.ContinueWith(t => {

						lockControls.Dispose();

						if (t.IsFaulted)
						{
							Debug.Assert(t.Exception != null);
							var ex = t.Exception.InnerException;
							MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						if (t.Status == TaskStatus.Canceled)
							return;

						SetArtifactsList(t.Result, DateTimeOffset.Now);

					}, TaskScheduler.FromCurrentSynchronizationContext())
				;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		class IterationFilterItem
		{
			public readonly IterationArtifact Artifact;

			public readonly string FilterName;

			static int _maxIterationNameLength;
			static readonly Regex NameShorter = new Regex(@"\s+iteration\s*#", RegexOptions.IgnoreCase);

			public readonly HashSet<long> IterationIds = new HashSet<long>();

			public IterationFilterItem(IterationArtifact itArt)
			{
				Artifact = itArt;

				if (Artifact == null)
				{
					FilterName = "All";
				}
				else if (Artifact.IsNullArtifact)
				{
					FilterName = "N/A";
					IterationIds.Add(0);
				}
				else
				{
					if (Artifact.IsCurrentIteration)
						FilterName = "=> ";
					else
						FilterName = "   ";

					var iterationName = GetIterationName(Artifact).PadRight(_maxIterationNameLength);

					FilterName = FilterName + iterationName + " [" + Artifact.StartDate.LocalDateTime.ToString("dd MMM") + " - " + Artifact.EndDate.LocalDateTime.ToString("dd MMM") + "]";

					IterationIds.Add(Artifact.ObjectID);
				}
			}

			static string GetIterationName(Artifact art)
			{
				if(art == null)
					return string.Empty;

				return NameShorter.Replace(art.Name ?? "", " It#");
			}

			// calculate pretty padding
			public static void ApplyAllIterationsList(List<IterationArtifact> arts)
			{
				if (arts.Count == 1)
					_maxIterationNameLength = GetIterationName(arts[0]).Length;
				else if (arts.Count > 1)
					_maxIterationNameLength = arts.Select(art => GetIterationName(art).Length).Max();
				else
					_maxIterationNameLength = 0;
			}

			public override string ToString()
			{
				return FilterName;
			}
		}

		void SetArtifactsList(List<Artifact> artifacts, DateTimeOffset updateTime)
		{
			comboBoxIterationsFilter.BeginUpdate();
			try
			{
				// set last update time
				simplifiedBackgroundOperation.Text = string.Format("Last refresh: {0:g}", updateTime);

				// prepare virtual items
				_allIssues.Clear();
				_allIssues.AddRange(artifacts.Select(a => new VirtualListItemArtifact(a, _columns)));

				// load iterations
				comboBoxIterationsFilter.Items.Clear();
				var itId2It = artifacts
					.Select(a => a.Iteration)
					.GroupBy(it => it.ObjectID)
					.Select(itg => itg.First())
					.ToDictionary(it => it.ObjectID, it => it)
				;

				IterationFilterItem.ApplyAllIterationsList(itId2It.Values.ToList());

				// default selection - all
				var selectedIndex = 0;
				comboBoxIterationsFilter.Items.Add(new IterationFilterItem(null));

				var itName2Filter = new Dictionary<string, IterationFilterItem>();

				foreach (var kvp in itId2It.OrderBy(kvp => kvp.Value.StartDate))
				{
					// it is possible situation when iteration name equal, but iteration objects - different (separate for each team)
					// merge them into one filter
					var filter = new IterationFilterItem(kvp.Value);
					IterationFilterItem existingFilter;
					if(itName2Filter.TryGetValue(filter.FilterName, out existingFilter))
					{
						existingFilter.IterationIds.Add(kvp.Value.ObjectID);
						continue;
					}
					comboBoxIterationsFilter.Items.Add(filter);
					itName2Filter[filter.FilterName] = filter;

					// skip selection if 'all' in preferences
					if (string.IsNullOrEmpty(Settings.Default.IterationFilter))
						continue;

					if (Settings.Default.IterationFilter == filter.FilterName)
					{
						selectedIndex = comboBoxIterationsFilter.Items.Count - 1;
						continue;
					}

					if (Settings.Default.IterationFilter == CurrentIterationMarker && kvp.Value.IsCurrentIteration)
					{
						selectedIndex = comboBoxIterationsFilter.Items.Count - 1;
						continue;
					}
				}

				// raise refill artifacts list
				comboBoxIterationsFilter.SelectedIndex = selectedIndex;
			}
			finally
			{
				comboBoxIterationsFilter.EndUpdate();
			}
		}

		IEnumerable<VirtualListItemArtifact> ApplyFilters(List<VirtualListItemArtifact> all)
		{
			var itFilter = comboBoxIterationsFilter.SelectedItem as IterationFilterItem;

			IEnumerable<VirtualListItemArtifact> filtered = all;

			if (checkBoxHideDefectSubtasks.Checked)
				filtered = filtered.Where(a => a.WorkItem.Parent.Type != ArtifactType.Defect);

			filtered = FilterNoiseTasks(filtered);

			if (itFilter != null && itFilter.Artifact != null)
				filtered = filtered.Where(ar => itFilter.IterationIds.Contains(ar.WorkItem.Iteration.ObjectID));

			var nameFilter = textBoxNameFilter.Text.ToLowerInvariant();
			if (!string.IsNullOrWhiteSpace(nameFilter))
			{
				filtered = filtered.Where(ar => (ar.WorkItem.Name ?? "").ToLowerInvariant().Contains(nameFilter));
				textBoxNameFilter.BackColor = AppliedFilter;
			}
			else
			{
				textBoxNameFilter.BackColor = EmptyFilter;
			}

			return filtered;
		}

		IEnumerable<VirtualListItemArtifact> FilterNoiseTasks(IEnumerable<VirtualListItemArtifact> items)
		{
			checkBoxHideNoiseTasks.BackColor = BackColor;
			if (!checkBoxHideNoiseTasks.Checked || string.IsNullOrWhiteSpace(Settings.Default.NoiseTaskRegex))
				return items;

			try
			{
				var rx = new Regex(Settings.Default.NoiseTaskRegex, RegexOptions.IgnoreCase);
				return items.Where(i => !rx.IsMatch(i.WorkItem.Name));
			}
			catch
			{
				checkBoxHideNoiseTasks.BackColor = Color.LightPink;
				return items;
			}
		}

		void UpdateFilteredArtifactsList()
		{
			// save selection
			var selectedArtifacts = listViewIssues.SelectedIndices.Cast<int>().Select(i => _filteredIssues[i].WorkItem).ToList();
			var focusedArtifact = listViewIssues.FocusedItem != null ? _filteredIssues[listViewIssues.FocusedItem.Index].WorkItem : null;

			listViewIssues.BeginUpdate();
			try
			{
				// prepare filtered virtual items
				listViewIssues.VirtualListSize = 0;
				_filteredIssues.Clear();
				_filteredIssues.AddRange(ApplyFilters(_allIssues));

				// reset sorting
				_columns.SortColumn = -1;
				_columns.PrevSortColumn = -1;

				foreach (ColumnHeader hdr in listViewIssues.Columns)
					hdr.Text = ((ColumnBase)hdr.Tag).Header;

				// clear selection
				listViewIssues.SelectedIndices.Clear();

				listViewIssues.VirtualListSize = _filteredIssues.Count;

				// restore selection
				foreach (var old in selectedArtifacts)
				{
					for (var i = 0; i < _filteredIssues.Count; i++)
					{
						if (_filteredIssues[i].WorkItem.ObjectID == old.ObjectID)
						{
							listViewIssues.SelectedIndices.Add(i);
							break;
						}
					}
				}

				// restore focused
				if (focusedArtifact != null)
				{
					for (var i = 0; i < _filteredIssues.Count; i++)
					{
						if (_filteredIssues[i].WorkItem.ObjectID == focusedArtifact.ObjectID)
						{
							listViewIssues.FocusedItem = _filteredIssues[i].ListItem;
							listViewIssues.EnsureVisible(i);
							break;
						}
					}
				}

				// do not steal focus from name filter
				if(!textBoxNameFilter.Focused)
					listViewIssues.Focus();
			}
			finally
			{
				listViewIssues.EndUpdate();
			}
		}

		void comboBoxIterationsFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			var filter = comboBoxIterationsFilter.SelectedItem as IterationFilterItem;

			UpdateFilteredArtifactsList();

			// save selection
			if (filter == null || filter.Artifact == null)
				Settings.Default.IterationFilter = "";
			else if (filter.Artifact.IsCurrentIteration)
				Settings.Default.IterationFilter = CurrentIterationMarker;
			else
				Settings.Default.IterationFilter = filter.FilterName;

			Settings.Default.Save();
		}

		LoadArtifactsBgOperation StartOrPickupLoadArtifacts()
		{
			// pickup progressed background operation
			if (_firstArtifactsLoad != null)
			{
				var op = _firstArtifactsLoad;
				_firstArtifactsLoad = null;

				if (!op.Task.IsCompleted)
					return op;
			}

			return StartLoadArtifacts();
		}

		LoadArtifactsBgOperation StartLoadArtifacts()
		{
			var op = new LoadArtifactsBgOperation { Cts = new CancellationTokenSource() };

			op.Task = Task.Factory.StartNew(() => {
				try
				{
					return LoadArtifacts(op.Cts.Token);
				}
				finally
				{
					op.Cts.Dispose();
					op.Cts = null;
				}
			}, op.Cts.Token);

			simplifiedBackgroundOperation.Track(op.Task, "Refreshing...");

			return op;
		}

		// load issues list
		List<Artifact> LoadArtifacts(CancellationToken ct)
		{
			var artifacts = new List<DynamicJsonObject>();

			var restApi = new RallyRestApi(Settings.Default.RallyUser, Settings.Default.RallyPassword
				//, proxy: new WebProxy("localhost:8888", false)
				, cancellationToken: ct
			);

			var query = new Query(Settings.Default.RallyQuery);

			{
				backgroundRefreshOperation.SetProgress("Loading defects  ...");

				var request = new Request("defect");
				request.Limit = Int32.MaxValue;
				request.Fetch = new List<string> { "FormattedID", "Requirement", "Name", "Project", "ObjectID", "Iteration", "EndDate", "StartDate" };
				request.Order = "Iteration";
				request.Query = query;

				// perfrom query
				var response = restApi.Query(request);
				artifacts.AddRange(response.Results.Select(a => (DynamicJsonObject)a));
			}

			ct.ThrowIfCancellationRequested();

			{
				backgroundRefreshOperation.SetProgress("Loading tasks ...");

				var request = new Request("task");
				request.Limit = Int32.MaxValue;
				request.Fetch = new List<string> { "FormattedID", "WorkProduct", "Name", "Project", "ObjectID", "Iteration", "EndDate", "StartDate" };
				request.Order = "Iteration";
				request.Query = query;

				// perfrom query
				var response = restApi.Query(request);
				artifacts.AddRange(response.Results.Select(a => (DynamicJsonObject)a));
			}

			// update cache
			UpdateCache(artifacts);

			var list = artifacts
				.Select(w => new Artifact(w))
				.ToList()
			;

			// move defect tasks immediately after defects
			for (var i = 0; i < list.Count; i++)
			{
				if(list[i].Parent.Type == ArtifactType.Defect)
				{
					// find defect in list
					var parentIndex = list.FindIndex(a => a.ObjectID == list[i].Parent.ObjectID);

					// if parent found and parent is not direct preceder
					if (parentIndex != -1 && parentIndex != (i - 1))
					{
						var task = list[i];
						list.RemoveAt(i);
						list.Insert(parentIndex + 1, task);
					}
				}
			}

			return list;
		}

		void UpdateCache(IEnumerable<DynamicJsonObject> artifacts)
		{
			var d = new Dictionary<string, object>();
			d["Query"] = textBoxQuery.Text;
			d["UpdateTime"] = DateTimeOffset.Now.Ticks.ToString(CultureInfo.InvariantCulture);
			d["Artifacts"] = artifacts.Select(djo => djo.ToDictionary()).ToList();
			var cache = JsonConvert.SerializeObject(d, Formatting.Indented);

			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.AppDataDirName);
			Directory.CreateDirectory(dir);

			var path = Path.Combine(dir, "cache.json");

			File.WriteAllText(path, cache);
		}

		List<Artifact> LoadCachedArtifacts(ref DateTimeOffset update)
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.AppDataDirName, "cache.json");

			if (!File.Exists(path))
				return null;

			try
			{
				var json = File.ReadAllText(path);

				var dict = new JavaScriptSerializer { MaxJsonLength = 10 * 1024 * 1024 }.Deserialize<Dictionary<string, object>>(json);

				if ((string)dict["Query"] != textBoxQuery.Text)
					return null;

				update = new DateTimeOffset(long.Parse((string)dict["UpdateTime"]), TimeSpan.Zero);

				// cache expired
				if ((update - DateTimeOffset.Now) > TimeSpan.FromDays(1))
					return null;

				return ((ArrayList)dict["Artifacts"])
					.Cast<Dictionary<string, object>>()
					.Select(d => new Artifact(new DynamicJsonObject(d)))
					.ToList()
				;
			}
			catch (Exception)
			{
			}

			return null;
		}

		ColumnsCollection InitColumns()
		{
			// init list view with columns
			var columns = new ColumnsCollection();
			var cols = new List<ColumnBase> {
				new FormattedIDColumn { Width = 90 },
				new IterationColumn { Width = 150 },
				new WorkItemNameColumn { Width = 400 },
				new ParentColumn { Width = 400 }
			};

			columns.Columns = cols.ToArray();

			listViewIssues.Columns.Clear();
			var colHdrs = columns
				.Columns
				.Select(col => new ColumnHeader {
					TextAlign = col.TextAlign,
					Text = col.Header,
					Width = col.Width,
					Tag = col
				})
				.ToArray()
			;

			listViewIssues.Columns.AddRange(colHdrs);

			return columns;
		}

		void listViewIssues_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = _filteredIssues[e.ItemIndex].ListItem;
		}

		void buttonOK_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolOK);
			GatherSelectedItems();
			DialogResult = DialogResult.OK;
			Close();
		}

		void GatherSelectedItems()
		{
			SelectedWorkItems = listViewIssues.SelectedIndices.Cast<int>().Select(i => _filteredIssues[i].WorkItem).ToArray();
		}

		void listViewIssues_DoubleClick(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolItemDblClick);

			if (_selectIssuesMode)
			{
				GatherSelectedItems();
				DialogResult = DialogResult.OK;
				return;
			}

			var wi = _filteredIssues[listViewIssues.FocusedItem.Index].WorkItem;

			if(wi.Url != null)
				Process.Start(wi.Url);
		}

		void buttonApplyQuery_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolApplyQuery);
			Settings.Default.Save();
			RefreshArtifactsList();
		}

		void sourceSafeCommitsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolSSCommits);

			if(_vssPowerToolsPath == null)
			{
				Process.Start("https://github.com/azarkevich/VssPowerTools/releases");
			}
			else
			{
				var p = new Process { StartInfo = new ProcessStartInfo { FileName = _vssPowerToolsPath } };
				p.Start();
			}
		}

		void buildsListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolStartBB);
			Process.Start(Settings.Default.BuildsBoardUri);
		}

		void svnLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolStartSvnLog);
			new SvnLog(_commonRoot).ShowDialog(this);
		}

		void dailyReportGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolStartDailyReport);
			new DailyReport().ShowDialog(this);
		}

		void openOnRallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolContextMenuOpenOnRally);
			listViewIssues
				.SelectedIndices
				.Cast<int>()
				.Select(i => _filteredIssues[i].WorkItem)
				.ToList()
				.ForEach(a => Process.Start(a.Url))
			;
		}

		void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolContextMenuCopy);
			var names = listViewIssues
				.SelectedIndices
				.Cast<int>()
				.Select(i => _filteredIssues[i].WorkItem)
				.Select(arr => string.Format("{0} {1}", arr.FormattedID, arr.Name))
				.ToArray()
			;

			var text = string.Join("\n", names);

			Clipboard.SetText(text);
		}

		void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolContextMenuRefresh);
			RefreshArtifactsList();
		}

		void buttonIterationFilterPrev_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolIterationFilterPrev);
			if (comboBoxIterationsFilter.SelectedIndex > 0)
				comboBoxIterationsFilter.SelectedIndex--;
		}

		void buttonIterationFilterNext_Click(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolIterationFilterNext);
			if (comboBoxIterationsFilter.SelectedIndex < (comboBoxIterationsFilter.Items.Count - 1))
				comboBoxIterationsFilter.SelectedIndex++;
		}

		void textBoxNameFilter_TextChanged(object sender, EventArgs e)
		{

		}

		void checkBoxHideDefectSubtasks_CheckedChanged(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolShowHideDefectsSubtasks);
			UpdateFilteredArtifactsList();
		}

		private void checkBoxHideNoiseTasks_CheckedChanged(object sender, EventArgs e)
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallyToolShowHideNoiseTasks);
			UpdateFilteredArtifactsList();
		}
	}
}
