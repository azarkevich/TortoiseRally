namespace TrackGearLibrary.Rally
{
	partial class RallySettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RallySettings));
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageRallySettings = new System.Windows.Forms.TabPage();
			this.textBoxNoiseTasksRegex = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBoxRallyCredsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanelRallyCreds = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxUserName = new System.Windows.Forms.TextBox();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.labelNote = new System.Windows.Forms.Label();
			this.tabPageAdvanced = new System.Windows.Forms.TabPage();
			this.textBoxLatestBuildURLTemplate = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textBoxReviewRequestTemplate = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxDailyReportTemplatePath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabPageRallySettings.SuspendLayout();
			this.groupBoxRallyCredsGroup.SuspendLayout();
			this.tableLayoutPanelRallyCreds.SuspendLayout();
			this.tabPageAdvanced.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(344, 293);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(425, 293);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageRallySettings);
			this.tabControl.Controls.Add(this.tabPageAdvanced);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(488, 266);
			this.tabControl.TabIndex = 9;
			// 
			// tabPageRallySettings
			// 
			this.tabPageRallySettings.Controls.Add(this.textBoxNoiseTasksRegex);
			this.tabPageRallySettings.Controls.Add(this.label5);
			this.tabPageRallySettings.Controls.Add(this.groupBoxRallyCredsGroup);
			this.tabPageRallySettings.Location = new System.Drawing.Point(4, 22);
			this.tabPageRallySettings.Name = "tabPageRallySettings";
			this.tabPageRallySettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRallySettings.Size = new System.Drawing.Size(480, 240);
			this.tabPageRallySettings.TabIndex = 0;
			this.tabPageRallySettings.Text = "Rally Settings";
			this.tabPageRallySettings.UseVisualStyleBackColor = true;
			this.tabPageRallySettings.Click += new System.EventHandler(this.tabPage1_Click);
			// 
			// textBoxNoiseTasksRegex
			// 
			this.textBoxNoiseTasksRegex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNoiseTasksRegex.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "NoiseTaskRegex", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxNoiseTasksRegex.Location = new System.Drawing.Point(6, 188);
			this.textBoxNoiseTasksRegex.Name = "textBoxNoiseTasksRegex";
			this.textBoxNoiseTasksRegex.Size = new System.Drawing.Size(465, 20);
			this.textBoxNoiseTasksRegex.TabIndex = 15;
			this.textBoxNoiseTasksRegex.Text = global::RallyToolsCore.Properties.Settings.Default.NoiseTaskRegex;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 172);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(104, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "\'Noise\' tasks RegEx:";
			// 
			// groupBoxRallyCredsGroup
			// 
			this.groupBoxRallyCredsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxRallyCredsGroup.Controls.Add(this.tableLayoutPanelRallyCreds);
			this.groupBoxRallyCredsGroup.Location = new System.Drawing.Point(9, 6);
			this.groupBoxRallyCredsGroup.Name = "groupBoxRallyCredsGroup";
			this.groupBoxRallyCredsGroup.Size = new System.Drawing.Size(465, 153);
			this.groupBoxRallyCredsGroup.TabIndex = 9;
			this.groupBoxRallyCredsGroup.TabStop = false;
			this.groupBoxRallyCredsGroup.Text = "Rally Credentials";
			// 
			// tableLayoutPanelRallyCreds
			// 
			this.tableLayoutPanelRallyCreds.ColumnCount = 3;
			this.tableLayoutPanelRallyCreds.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanelRallyCreds.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanelRallyCreds.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
			this.tableLayoutPanelRallyCreds.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanelRallyCreds.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanelRallyCreds.Controls.Add(this.textBoxUserName, 1, 1);
			this.tableLayoutPanelRallyCreds.Controls.Add(this.textBoxPassword, 1, 2);
			this.tableLayoutPanelRallyCreds.Controls.Add(this.labelNote, 0, 0);
			this.tableLayoutPanelRallyCreds.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelRallyCreds.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanelRallyCreds.Name = "tableLayoutPanelRallyCreds";
			this.tableLayoutPanelRallyCreds.RowCount = 4;
			this.tableLayoutPanelRallyCreds.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanelRallyCreds.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelRallyCreds.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelRallyCreds.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelRallyCreds.Size = new System.Drawing.Size(459, 134);
			this.tableLayoutPanelRallyCreds.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 26);
			this.label1.TabIndex = 1;
			this.label1.Text = "User";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 26);
			this.label2.TabIndex = 3;
			this.label2.Text = "Password";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxUserName
			// 
			this.tableLayoutPanelRallyCreds.SetColumnSpan(this.textBoxUserName, 2);
			this.textBoxUserName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "RallyUser", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxUserName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxUserName.Location = new System.Drawing.Point(62, 23);
			this.textBoxUserName.Name = "textBoxUserName";
			this.textBoxUserName.Size = new System.Drawing.Size(394, 20);
			this.textBoxUserName.TabIndex = 2;
			this.textBoxUserName.Text = global::RallyToolsCore.Properties.Settings.Default.RallyUser;
			// 
			// textBoxPassword
			// 
			this.tableLayoutPanelRallyCreds.SetColumnSpan(this.textBoxPassword, 2);
			this.textBoxPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "RallyPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxPassword.Location = new System.Drawing.Point(62, 49);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.PasswordChar = '*';
			this.textBoxPassword.Size = new System.Drawing.Size(394, 20);
			this.textBoxPassword.TabIndex = 4;
			this.textBoxPassword.Text = global::RallyToolsCore.Properties.Settings.Default.RallyPassword;
			this.textBoxPassword.UseSystemPasswordChar = true;
			// 
			// labelNote
			// 
			this.tableLayoutPanelRallyCreds.SetColumnSpan(this.labelNote, 3);
			this.labelNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelNote.Location = new System.Drawing.Point(3, 0);
			this.labelNote.Name = "labelNote";
			this.labelNote.Size = new System.Drawing.Size(453, 20);
			this.labelNote.TabIndex = 0;
			this.labelNote.Text = "Rally login and password:";
			this.labelNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabPageAdvanced
			// 
			this.tabPageAdvanced.Controls.Add(this.textBox1);
			this.tabPageAdvanced.Controls.Add(this.label7);
			this.tabPageAdvanced.Controls.Add(this.textBoxLatestBuildURLTemplate);
			this.tabPageAdvanced.Controls.Add(this.label6);
			this.tabPageAdvanced.Controls.Add(this.textBoxReviewRequestTemplate);
			this.tabPageAdvanced.Controls.Add(this.label4);
			this.tabPageAdvanced.Controls.Add(this.textBoxDailyReportTemplatePath);
			this.tabPageAdvanced.Controls.Add(this.label3);
			this.tabPageAdvanced.Controls.Add(this.checkBox1);
			this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tabPageAdvanced.Name = "tabPageAdvanced";
			this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageAdvanced.Size = new System.Drawing.Size(480, 240);
			this.tabPageAdvanced.TabIndex = 1;
			this.tabPageAdvanced.Text = "Advanced";
			this.tabPageAdvanced.UseVisualStyleBackColor = true;
			// 
			// textBoxLatestBuildURLTemplate
			// 
			this.textBoxLatestBuildURLTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLatestBuildURLTemplate.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "NextBuildUrlTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxLatestBuildURLTemplate.Location = new System.Drawing.Point(6, 149);
			this.textBoxLatestBuildURLTemplate.Name = "textBoxLatestBuildURLTemplate";
			this.textBoxLatestBuildURLTemplate.Size = new System.Drawing.Size(465, 20);
			this.textBoxLatestBuildURLTemplate.TabIndex = 19;
			this.textBoxLatestBuildURLTemplate.Text = global::RallyToolsCore.Properties.Settings.Default.NextBuildUrlTemplate;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 133);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(132, 13);
			this.label6.TabIndex = 18;
			this.label6.Text = "Latest build URL template:";
			// 
			// textBoxReviewRequestTemplate
			// 
			this.textBoxReviewRequestTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxReviewRequestTemplate.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "ReviewRequestTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxReviewRequestTemplate.Location = new System.Drawing.Point(6, 101);
			this.textBoxReviewRequestTemplate.Name = "textBoxReviewRequestTemplate";
			this.textBoxReviewRequestTemplate.Size = new System.Drawing.Size(465, 20);
			this.textBoxReviewRequestTemplate.TabIndex = 17;
			this.textBoxReviewRequestTemplate.Text = global::RallyToolsCore.Properties.Settings.Default.ReviewRequestTemplate;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 85);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(202, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "Review Request template path (optional):";
			// 
			// textBoxDailyReportTemplatePath
			// 
			this.textBoxDailyReportTemplatePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDailyReportTemplatePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "DailyReportTemplatePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBoxDailyReportTemplatePath.Location = new System.Drawing.Point(6, 62);
			this.textBoxDailyReportTemplatePath.Name = "textBoxDailyReportTemplatePath";
			this.textBoxDailyReportTemplatePath.Size = new System.Drawing.Size(465, 20);
			this.textBoxDailyReportTemplatePath.TabIndex = 15;
			this.textBoxDailyReportTemplatePath.Text = global::RallyToolsCore.Properties.Settings.Default.DailyReportTemplatePath;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(181, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Daily Report template path (optional):";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = global::RallyToolsCore.Properties.Settings.Default.EnablePostCommitTools;
			this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RallyToolsCore.Properties.Settings.Default, "EnablePostCommitTools", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox1.Location = new System.Drawing.Point(6, 6);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(148, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Enable \'Post Commit Tool\'";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::RallyToolsCore.Properties.Settings.Default, "BuildsBoardUri", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox1.Location = new System.Drawing.Point(6, 197);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(465, 20);
			this.textBox1.TabIndex = 21;
			this.textBox1.Text = global::RallyToolsCore.Properties.Settings.Default.BuildsBoardUri;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 181);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(78, 13);
			this.label7.TabIndex = 20;
			this.label7.Text = "Builds list URL:";
			// 
			// RallySettings
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(512, 328);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RallySettings";
			this.Text = "Rally Settings";
			this.Load += new System.EventHandler(this.RallySettings_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPageRallySettings.ResumeLayout(false);
			this.tabPageRallySettings.PerformLayout();
			this.groupBoxRallyCredsGroup.ResumeLayout(false);
			this.tableLayoutPanelRallyCreds.ResumeLayout(false);
			this.tableLayoutPanelRallyCreds.PerformLayout();
			this.tabPageAdvanced.ResumeLayout(false);
			this.tabPageAdvanced.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageRallySettings;
		private System.Windows.Forms.TextBox textBoxNoiseTasksRegex;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBoxRallyCredsGroup;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRallyCreds;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxUserName;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label labelNote;
		private System.Windows.Forms.TabPage tabPageAdvanced;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.TextBox textBoxReviewRequestTemplate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxDailyReportTemplatePath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxLatestBuildURLTemplate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label7;
	}
}