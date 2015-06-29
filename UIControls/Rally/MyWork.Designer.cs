namespace TrackGearLibrary.Rally
{
	partial class DailyReport
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyReport));
			this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
			this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonLoadChanges = new System.Windows.Forms.Button();
			this.listViewChanged = new System.Windows.Forms.ListView();
			this.colProject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colParentName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFormattedID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDonePercents = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colChanges = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openInRallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonGenerate = new System.Windows.Forms.Button();
			this.comboBoxDatePreset = new System.Windows.Forms.ComboBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.textBoxUser = new System.Windows.Forms.TextBox();
			this.backgroundOperation = new UIControls.BackgroundOperation();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// dateTimePickerFrom
			// 
			this.dateTimePickerFrom.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePickerFrom.Location = new System.Drawing.Point(346, 11);
			this.dateTimePickerFrom.Name = "dateTimePickerFrom";
			this.dateTimePickerFrom.Size = new System.Drawing.Size(148, 20);
			this.dateTimePickerFrom.TabIndex = 1;
			this.dateTimePickerFrom.ValueChanged += new System.EventHandler(this.dateTimePickerFrom_ValueChanged);
			// 
			// dateTimePickerTo
			// 
			this.dateTimePickerTo.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePickerTo.Location = new System.Drawing.Point(529, 11);
			this.dateTimePickerTo.Name = "dateTimePickerTo";
			this.dateTimePickerTo.Size = new System.Drawing.Size(146, 20);
			this.dateTimePickerTo.TabIndex = 3;
			this.dateTimePickerTo.ValueChanged += new System.EventHandler(this.dateTimePickerTo_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(308, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(500, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "To:";
			// 
			// buttonLoadChanges
			// 
			this.buttonLoadChanges.Location = new System.Drawing.Point(811, 12);
			this.buttonLoadChanges.Name = "buttonLoadChanges";
			this.buttonLoadChanges.Size = new System.Drawing.Size(75, 20);
			this.buttonLoadChanges.TabIndex = 5;
			this.buttonLoadChanges.Text = "Load";
			this.buttonLoadChanges.UseVisualStyleBackColor = true;
			this.buttonLoadChanges.Click += new System.EventHandler(this.buttonLoadChanges_Click);
			// 
			// listViewChanged
			// 
			this.listViewChanged.AllowColumnReorder = true;
			this.listViewChanged.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewChanged.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colProject,
            this.colOwner,
            this.colParentName,
            this.colFormattedID,
            this.colName,
            this.colState,
            this.colDonePercents,
            this.colChanges});
			this.listViewChanged.ContextMenuStrip = this.contextMenuStrip;
			this.listViewChanged.FullRowSelect = true;
			this.listViewChanged.GridLines = true;
			this.listViewChanged.HideSelection = false;
			this.listViewChanged.Location = new System.Drawing.Point(12, 38);
			this.listViewChanged.Name = "listViewChanged";
			this.listViewChanged.ShowItemToolTips = true;
			this.listViewChanged.Size = new System.Drawing.Size(1062, 383);
			this.listViewChanged.TabIndex = 6;
			this.listViewChanged.UseCompatibleStateImageBehavior = false;
			this.listViewChanged.View = System.Windows.Forms.View.Details;
			// 
			// colProject
			// 
			this.colProject.Text = "Project";
			this.colProject.Width = 45;
			// 
			// colOwner
			// 
			this.colOwner.Text = "Owner";
			this.colOwner.Width = 138;
			// 
			// colParentName
			// 
			this.colParentName.Text = "Parent";
			this.colParentName.Width = 84;
			// 
			// colFormattedID
			// 
			this.colFormattedID.Text = "FormattedID";
			this.colFormattedID.Width = 78;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 452;
			// 
			// colState
			// 
			this.colState.Text = "State";
			this.colState.Width = 94;
			// 
			// colDonePercents
			// 
			this.colDonePercents.Text = "Done, %";
			this.colDonePercents.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// colChanges
			// 
			this.colChanges.Text = "Changes";
			this.colChanges.Width = 103;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.openInRallyToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(145, 48);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// openInRallyToolStripMenuItem
			// 
			this.openInRallyToolStripMenuItem.Name = "openInRallyToolStripMenuItem";
			this.openInRallyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.openInRallyToolStripMenuItem.Text = "Open in Rally";
			this.openInRallyToolStripMenuItem.Click += new System.EventHandler(this.openInRallyToolStripMenuItem_Click);
			// 
			// buttonGenerate
			// 
			this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGenerate.Location = new System.Drawing.Point(999, 427);
			this.buttonGenerate.Name = "buttonGenerate";
			this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
			this.buttonGenerate.TabIndex = 8;
			this.buttonGenerate.Text = "Generate";
			this.buttonGenerate.UseVisualStyleBackColor = true;
			this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
			// 
			// comboBoxDatePreset
			// 
			this.comboBoxDatePreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDatePreset.FormattingEnabled = true;
			this.comboBoxDatePreset.Items.AddRange(new object[] {
            "Today",
            "24 hours",
            "Yesterday"});
			this.comboBoxDatePreset.Location = new System.Drawing.Point(684, 12);
			this.comboBoxDatePreset.Name = "comboBoxDatePreset";
			this.comboBoxDatePreset.Size = new System.Drawing.Size(121, 21);
			this.comboBoxDatePreset.TabIndex = 4;
			this.comboBoxDatePreset.SelectedIndexChanged += new System.EventHandler(this.comboBoxDatePreset_SelectedIndexChanged);
			// 
			// toolTip
			// 
			this.toolTip.ShowAlways = true;
			this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			// 
			// textBoxUser
			// 
			this.textBoxUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBoxUser.ForeColor = System.Drawing.SystemColors.ScrollBar;
			this.textBoxUser.Location = new System.Drawing.Point(12, 12);
			this.textBoxUser.Name = "textBoxUser";
			this.textBoxUser.Size = new System.Drawing.Size(278, 20);
			this.textBoxUser.TabIndex = 9;
			this.textBoxUser.Tag = "placeholder";
			this.textBoxUser.Text = "User Login...";
			this.textBoxUser.TextChanged += new System.EventHandler(this.textBoxUser_TextChanged);
			this.textBoxUser.Enter += new System.EventHandler(this.textBoxUser_Enter);
			// 
			// backgroundOperation
			// 
			this.backgroundOperation.AbandonOnCancel = true;
			this.backgroundOperation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.backgroundOperation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.backgroundOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.backgroundOperation.CloseOnRanToCompletion = true;
			this.backgroundOperation.Location = new System.Drawing.Point(415, 164);
			this.backgroundOperation.Name = "backgroundOperation";
			this.backgroundOperation.Size = new System.Drawing.Size(247, 117);
			this.backgroundOperation.TabIndex = 7;
			this.backgroundOperation.Visible = false;
			this.backgroundOperation.WaitMessage = "Please wait ...";
			// 
			// DailyReport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1086, 462);
			this.Controls.Add(this.textBoxUser);
			this.Controls.Add(this.comboBoxDatePreset);
			this.Controls.Add(this.buttonGenerate);
			this.Controls.Add(this.buttonLoadChanges);
			this.Controls.Add(this.backgroundOperation);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dateTimePickerTo);
			this.Controls.Add(this.dateTimePickerFrom);
			this.Controls.Add(this.listViewChanged);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "DailyReport";
			this.Text = "My Work";
			this.Load += new System.EventHandler(this.DailyReport_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DailyReport_KeyUp);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.DailyReport_PreviewKeyDown);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
		private System.Windows.Forms.DateTimePicker dateTimePickerTo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private UIControls.BackgroundOperation backgroundOperation;
		private System.Windows.Forms.Button buttonLoadChanges;
		private System.Windows.Forms.ListView listViewChanged;
		private System.Windows.Forms.ColumnHeader colFormattedID;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colChanges;
		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.ColumnHeader colParentName;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboBoxDatePreset;
		private System.Windows.Forms.ColumnHeader colState;
		private System.Windows.Forms.ColumnHeader colDonePercents;
		private System.Windows.Forms.ToolStripMenuItem openInRallyToolStripMenuItem;
		private System.Windows.Forms.ColumnHeader colOwner;
		private System.Windows.Forms.ColumnHeader colProject;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TextBox textBoxUser;
	}
}