namespace TrackGearLibrary.Rally
{
	partial class RallyTool
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RallyTool));
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sourceSafeCommitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buildsListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.svnLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dailyReportGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listViewIssues = new System.Windows.Forms.ListView();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openOnRallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonApplyQuery = new System.Windows.Forms.Button();
			this.versionPlaceholder = new System.Windows.Forms.Label();
			this.comboBoxIterationsFilter = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonIterationFilterPrev = new System.Windows.Forms.Button();
			this.buttonIterationFilterNext = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxNameFilter = new System.Windows.Forms.TextBox();
			this.simplifiedBackgroundOperation = new UIControls.SimplifiedBackgroundOperation();
			this.backgroundRefreshOperation = new UIControls.BackgroundOperation();
			this.checkBoxHideNoiseTasks = new System.Windows.Forms.CheckBox();
			this.checkBoxHideDefectSubtasks = new System.Windows.Forms.CheckBox();
			this.textBoxQuery = new System.Windows.Forms.TextBox();
			this.mainMenu.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(1144, 24);
			this.mainMenu.TabIndex = 5;
			this.mainMenu.Text = "menuStrip1";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
			this.optionsToolStripMenuItem.Text = "Options ...";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceSafeCommitsToolStripMenuItem,
            this.buildsListToolStripMenuItem,
            this.svnLogToolStripMenuItem,
            this.dailyReportGeneratorToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// sourceSafeCommitsToolStripMenuItem
			// 
			this.sourceSafeCommitsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sourceSafeCommitsToolStripMenuItem.Image")));
			this.sourceSafeCommitsToolStripMenuItem.Name = "sourceSafeCommitsToolStripMenuItem";
			this.sourceSafeCommitsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.sourceSafeCommitsToolStripMenuItem.Text = "SourceSafe tools...";
			this.sourceSafeCommitsToolStripMenuItem.Click += new System.EventHandler(this.sourceSafeCommitsToolStripMenuItem_Click);
			// 
			// buildsListToolStripMenuItem
			// 
			this.buildsListToolStripMenuItem.Name = "buildsListToolStripMenuItem";
			this.buildsListToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.buildsListToolStripMenuItem.Text = "Builds && branches list...";
			this.buildsListToolStripMenuItem.Click += new System.EventHandler(this.buildsListToolStripMenuItem_Click);
			// 
			// svnLogToolStripMenuItem
			// 
			this.svnLogToolStripMenuItem.Image = global::RallyToolsCore.Properties.Resources.subversion;
			this.svnLogToolStripMenuItem.Name = "svnLogToolStripMenuItem";
			this.svnLogToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.svnLogToolStripMenuItem.Text = "Svn Log...";
			this.svnLogToolStripMenuItem.Click += new System.EventHandler(this.svnLogToolStripMenuItem_Click);
			// 
			// dailyReportGeneratorToolStripMenuItem
			// 
			this.dailyReportGeneratorToolStripMenuItem.Name = "dailyReportGeneratorToolStripMenuItem";
			this.dailyReportGeneratorToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.dailyReportGeneratorToolStripMenuItem.Text = "My Work ...";
			this.dailyReportGeneratorToolStripMenuItem.Click += new System.EventHandler(this.dailyReportGeneratorToolStripMenuItem_Click);
			// 
			// listViewIssues
			// 
			this.listViewIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewIssues.ContextMenuStrip = this.contextMenuStrip;
			this.listViewIssues.FullRowSelect = true;
			this.listViewIssues.GridLines = true;
			this.listViewIssues.HideSelection = false;
			this.listViewIssues.Location = new System.Drawing.Point(12, 102);
			this.listViewIssues.Name = "listViewIssues";
			this.listViewIssues.Size = new System.Drawing.Size(1120, 351);
			this.listViewIssues.TabIndex = 1;
			this.listViewIssues.UseCompatibleStateImageBehavior = false;
			this.listViewIssues.View = System.Windows.Forms.View.Details;
			this.listViewIssues.VirtualMode = true;
			this.listViewIssues.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewIssues_RetrieveVirtualItem);
			this.listViewIssues.DoubleClick += new System.EventHandler(this.listViewIssues_DoubleClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOnRallyToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.refreshToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(161, 70);
			// 
			// openOnRallyToolStripMenuItem
			// 
			this.openOnRallyToolStripMenuItem.Name = "openOnRallyToolStripMenuItem";
			this.openOnRallyToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.openOnRallyToolStripMenuItem.Text = "Open on Rally ...";
			this.openOnRallyToolStripMenuItem.Click += new System.EventHandler(this.openOnRallyToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(1057, 459);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "Select";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Query:";
			// 
			// buttonApplyQuery
			// 
			this.buttonApplyQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApplyQuery.Location = new System.Drawing.Point(710, 24);
			this.buttonApplyQuery.Name = "buttonApplyQuery";
			this.buttonApplyQuery.Size = new System.Drawing.Size(75, 23);
			this.buttonApplyQuery.TabIndex = 8;
			this.buttonApplyQuery.Text = "Apply";
			this.buttonApplyQuery.UseVisualStyleBackColor = true;
			this.buttonApplyQuery.Click += new System.EventHandler(this.buttonApplyQuery_Click);
			// 
			// versionPlaceholder
			// 
			this.versionPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.versionPlaceholder.Location = new System.Drawing.Point(791, 24);
			this.versionPlaceholder.Name = "versionPlaceholder";
			this.versionPlaceholder.Size = new System.Drawing.Size(341, 23);
			this.versionPlaceholder.TabIndex = 9;
			this.versionPlaceholder.Text = "versionPlaceholder";
			this.versionPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBoxIterationsFilter
			// 
			this.comboBoxIterationsFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxIterationsFilter.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.comboBoxIterationsFilter.FormattingEnabled = true;
			this.comboBoxIterationsFilter.Location = new System.Drawing.Point(89, 52);
			this.comboBoxIterationsFilter.Name = "comboBoxIterationsFilter";
			this.comboBoxIterationsFilter.Size = new System.Drawing.Size(367, 21);
			this.comboBoxIterationsFilter.TabIndex = 12;
			this.comboBoxIterationsFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxIterationsFilter_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Iter:";
			// 
			// buttonIterationFilterPrev
			// 
			this.buttonIterationFilterPrev.Location = new System.Drawing.Point(56, 52);
			this.buttonIterationFilterPrev.Name = "buttonIterationFilterPrev";
			this.buttonIterationFilterPrev.Size = new System.Drawing.Size(27, 21);
			this.buttonIterationFilterPrev.TabIndex = 11;
			this.buttonIterationFilterPrev.Text = "<<";
			this.buttonIterationFilterPrev.UseVisualStyleBackColor = true;
			this.buttonIterationFilterPrev.Click += new System.EventHandler(this.buttonIterationFilterPrev_Click);
			// 
			// buttonIterationFilterNext
			// 
			this.buttonIterationFilterNext.Location = new System.Drawing.Point(462, 52);
			this.buttonIterationFilterNext.Name = "buttonIterationFilterNext";
			this.buttonIterationFilterNext.Size = new System.Drawing.Size(27, 21);
			this.buttonIterationFilterNext.TabIndex = 13;
			this.buttonIterationFilterNext.Text = ">>";
			this.buttonIterationFilterNext.UseVisualStyleBackColor = true;
			this.buttonIterationFilterNext.Click += new System.EventHandler(this.buttonIterationFilterNext_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(495, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Name:";
			// 
			// textBoxNameFilter
			// 
			this.textBoxNameFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNameFilter.Location = new System.Drawing.Point(539, 53);
			this.textBoxNameFilter.Name = "textBoxNameFilter";
			this.textBoxNameFilter.Size = new System.Drawing.Size(593, 20);
			this.textBoxNameFilter.TabIndex = 0;
			this.textBoxNameFilter.TextChanged += new System.EventHandler(this.textBoxNameFilter_TextChanged);
			// 
			// simplifiedBackgroundOperation
			// 
			this.simplifiedBackgroundOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.simplifiedBackgroundOperation.AutoSize = true;
			this.simplifiedBackgroundOperation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.simplifiedBackgroundOperation.Location = new System.Drawing.Point(12, 460);
			this.simplifiedBackgroundOperation.Name = "simplifiedBackgroundOperation";
			this.simplifiedBackgroundOperation.Size = new System.Drawing.Size(63, 21);
			this.simplifiedBackgroundOperation.TabIndex = 3;
			// 
			// backgroundRefreshOperation
			// 
			this.backgroundRefreshOperation.AbandonOnCancel = true;
			this.backgroundRefreshOperation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.backgroundRefreshOperation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.backgroundRefreshOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.backgroundRefreshOperation.CloseOnRanToCompletion = true;
			this.backgroundRefreshOperation.Location = new System.Drawing.Point(440, 197);
			this.backgroundRefreshOperation.Name = "backgroundRefreshOperation";
			this.backgroundRefreshOperation.Size = new System.Drawing.Size(247, 100);
			this.backgroundRefreshOperation.TabIndex = 2;
			this.backgroundRefreshOperation.Visible = false;
			this.backgroundRefreshOperation.WaitMessage = "Loading Items ...";
			// 
			// checkBoxHideNoiseTasks
			// 
			this.checkBoxHideNoiseTasks.AutoSize = true;
			this.checkBoxHideNoiseTasks.Checked = global::RallyToolsCore.Properties.Settings.Default.HideNoiseTasks;
			this.checkBoxHideNoiseTasks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxHideNoiseTasks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RallyToolsCore.Properties.Settings.Default, "HideNoiseTasks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBoxHideNoiseTasks.Location = new System.Drawing.Point(190, 79);
			this.checkBoxHideNoiseTasks.Name = "checkBoxHideNoiseTasks";
			this.checkBoxHideNoiseTasks.Size = new System.Drawing.Size(108, 17);
			this.checkBoxHideNoiseTasks.TabIndex = 15;
			this.checkBoxHideNoiseTasks.Text = "Hide \'noise\' tasks";
			this.checkBoxHideNoiseTasks.UseVisualStyleBackColor = true;
			this.checkBoxHideNoiseTasks.CheckedChanged += new System.EventHandler(this.checkBoxHideNoiseTasks_CheckedChanged);
			// 
			// checkBoxHideDefectSubtasks
			// 
			this.checkBoxHideDefectSubtasks.AutoSize = true;
			this.checkBoxHideDefectSubtasks.Checked = global::RallyToolsCore.Properties.Settings.Default.HideDefectSubtasks;
			this.checkBoxHideDefectSubtasks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxHideDefectSubtasks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RallyToolsCore.Properties.Settings.Default, "HideDefectSubtasks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBoxHideDefectSubtasks.Location = new System.Drawing.Point(56, 79);
			this.checkBoxHideDefectSubtasks.Name = "checkBoxHideDefectSubtasks";
			this.checkBoxHideDefectSubtasks.Size = new System.Drawing.Size(128, 17);
			this.checkBoxHideDefectSubtasks.TabIndex = 15;
			this.checkBoxHideDefectSubtasks.Text = "Hide Defect subtasks";
			this.checkBoxHideDefectSubtasks.UseVisualStyleBackColor = true;
			this.checkBoxHideDefectSubtasks.CheckedChanged += new System.EventHandler(this.checkBoxHideDefectSubtasks_CheckedChanged);
			// 
			// textBoxQuery
			// 
			this.textBoxQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxQuery.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "RallyQuery", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxQuery.Location = new System.Drawing.Point(56, 26);
			this.textBoxQuery.Name = "textBoxQuery";
			this.textBoxQuery.Size = new System.Drawing.Size(648, 20);
			this.textBoxQuery.TabIndex = 7;
			this.textBoxQuery.Text = global::RallyToolsCore.Properties.Settings.Default.RallyQuery;
			// 
			// RallyTool
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1144, 494);
			this.Controls.Add(this.checkBoxHideNoiseTasks);
			this.Controls.Add(this.checkBoxHideDefectSubtasks);
			this.Controls.Add(this.textBoxNameFilter);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.buttonIterationFilterNext);
			this.Controls.Add(this.buttonIterationFilterPrev);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxIterationsFilter);
			this.Controls.Add(this.simplifiedBackgroundOperation);
			this.Controls.Add(this.backgroundRefreshOperation);
			this.Controls.Add(this.versionPlaceholder);
			this.Controls.Add(this.buttonApplyQuery);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxQuery);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.listViewIssues);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenu;
			this.Name = "RallyTool";
			this.Text = "Rally Tool";
			this.Load += new System.EventHandler(this.RallyTool_Load);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ListView listViewIssues;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TextBox textBoxQuery;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonApplyQuery;
		private System.Windows.Forms.Label versionPlaceholder;
		private UIControls.BackgroundOperation backgroundRefreshOperation;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sourceSafeCommitsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildsListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem svnLogToolStripMenuItem;
		private UIControls.SimplifiedBackgroundOperation simplifiedBackgroundOperation;
		private System.Windows.Forms.ToolStripMenuItem dailyReportGeneratorToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboBoxIterationsFilter;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem openOnRallyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonIterationFilterPrev;
		private System.Windows.Forms.Button buttonIterationFilterNext;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxNameFilter;
		private System.Windows.Forms.CheckBox checkBoxHideDefectSubtasks;
		private System.Windows.Forms.CheckBox checkBoxHideNoiseTasks;
	}
}