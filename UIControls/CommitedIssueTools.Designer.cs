namespace TrackGearLibrary
{
	partial class CommitedIssueTools
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommitedIssueTools));
			this.buttonGenFDP = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonSendForReview = new System.Windows.Forms.Button();
			this.buttonGenFdpToFile = new System.Windows.Forms.Button();
			this.linkLabelRegressBuild = new System.Windows.Forms.LinkLabel();
			this.buttonComposeReviewRequestHtml = new System.Windows.Forms.Button();
			this.comboBoxReviewer = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.buttonPostFDP = new System.Windows.Forms.Button();
			this.bgOperationProgress = new UIControls.SimplifiedBackgroundOperation();
			this.fileSystemWatcherReviewers = new System.IO.FileSystemWatcher();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcherReviewers)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonGenFDP
			// 
			this.buttonGenFDP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGenFDP.Location = new System.Drawing.Point(12, 39);
			this.buttonGenFDP.Name = "buttonGenFDP";
			this.buttonGenFDP.Size = new System.Drawing.Size(253, 23);
			this.buttonGenFDP.TabIndex = 2;
			this.buttonGenFDP.Text = "Generate FDP to Clipboard";
			this.buttonGenFDP.UseVisualStyleBackColor = true;
			this.buttonGenFDP.Click += new System.EventHandler(this.buttonGenerateFDP_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(190, 160);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 7;
			this.buttonOk.Text = "Close";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonSendForReview
			// 
			this.buttonSendForReview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSendForReview.Location = new System.Drawing.Point(12, 97);
			this.buttonSendForReview.Name = "buttonSendForReview";
			this.buttonSendForReview.Size = new System.Drawing.Size(189, 23);
			this.buttonSendForReview.TabIndex = 4;
			this.buttonSendForReview.Text = "Compose review request (EMail)";
			this.buttonSendForReview.UseVisualStyleBackColor = true;
			this.buttonSendForReview.Click += new System.EventHandler(this.buttonSendForReview_Click);
			// 
			// buttonGenFdpToFile
			// 
			this.buttonGenFdpToFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGenFdpToFile.Location = new System.Drawing.Point(12, 68);
			this.buttonGenFdpToFile.Name = "buttonGenFdpToFile";
			this.buttonGenFdpToFile.Size = new System.Drawing.Size(189, 23);
			this.buttonGenFdpToFile.TabIndex = 3;
			this.buttonGenFdpToFile.Text = "Generate FDP to File";
			this.buttonGenFdpToFile.UseVisualStyleBackColor = true;
			this.buttonGenFdpToFile.Click += new System.EventHandler(this.buttonGenerateFDP_Click);
			// 
			// linkLabelRegressBuild
			// 
			this.linkLabelRegressBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkLabelRegressBuild.AutoSize = true;
			this.linkLabelRegressBuild.Location = new System.Drawing.Point(9, 164);
			this.linkLabelRegressBuild.Name = "linkLabelRegressBuild";
			this.linkLabelRegressBuild.Size = new System.Drawing.Size(85, 13);
			this.linkLabelRegressBuild.TabIndex = 6;
			this.linkLabelRegressBuild.TabStop = true;
			this.linkLabelRegressBuild.Text = "Build For regress";
			// 
			// buttonComposeReviewRequestHtml
			// 
			this.buttonComposeReviewRequestHtml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonComposeReviewRequestHtml.Location = new System.Drawing.Point(207, 97);
			this.buttonComposeReviewRequestHtml.Name = "buttonComposeReviewRequestHtml";
			this.buttonComposeReviewRequestHtml.Size = new System.Drawing.Size(58, 23);
			this.buttonComposeReviewRequestHtml.TabIndex = 5;
			this.buttonComposeReviewRequestHtml.Text = "HTML";
			this.buttonComposeReviewRequestHtml.UseVisualStyleBackColor = true;
			this.buttonComposeReviewRequestHtml.Click += new System.EventHandler(this.buttonComposeReviewRequestHtml_Click);
			// 
			// comboBoxReviewer
			// 
			this.comboBoxReviewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxReviewer.FormattingEnabled = true;
			this.comboBoxReviewer.Location = new System.Drawing.Point(70, 12);
			this.comboBoxReviewer.Name = "comboBoxReviewer";
			this.comboBoxReviewer.Size = new System.Drawing.Size(195, 21);
			this.comboBoxReviewer.TabIndex = 1;
			this.comboBoxReviewer.SelectedIndexChanged += new System.EventHandler(this.comboBoxReviewer_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Reviewer";
			// 
			// buttonPostFDP
			// 
			this.buttonPostFDP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPostFDP.Enabled = false;
			this.buttonPostFDP.Location = new System.Drawing.Point(207, 68);
			this.buttonPostFDP.Name = "buttonPostFDP";
			this.buttonPostFDP.Size = new System.Drawing.Size(58, 23);
			this.buttonPostFDP.TabIndex = 11;
			this.buttonPostFDP.Text = "Post";
			this.toolTip.SetToolTip(this.buttonPostFDP, "Post saved to temp file FDP to defect discussion\r\nAvailable only when one defect " +
        "is fixed.");
			this.buttonPostFDP.UseVisualStyleBackColor = true;
			this.buttonPostFDP.Click += new System.EventHandler(this.buttonPostFDP_Click);
			// 
			// bgOperationProgress
			// 
			this.bgOperationProgress.AutoSize = true;
			this.bgOperationProgress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.bgOperationProgress.Location = new System.Drawing.Point(12, 126);
			this.bgOperationProgress.Name = "bgOperationProgress";
			this.bgOperationProgress.Size = new System.Drawing.Size(63, 21);
			this.bgOperationProgress.TabIndex = 10;
			// 
			// fileSystemWatcherReviewers
			// 
			this.fileSystemWatcherReviewers.EnableRaisingEvents = true;
			this.fileSystemWatcherReviewers.SynchronizingObject = this;
			this.fileSystemWatcherReviewers.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcherReviewers_Changed);
			// 
			// CommitedIssueTools
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonOk;
			this.ClientSize = new System.Drawing.Size(277, 193);
			this.Controls.Add(this.buttonPostFDP);
			this.Controls.Add(this.bgOperationProgress);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxReviewer);
			this.Controls.Add(this.linkLabelRegressBuild);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonComposeReviewRequestHtml);
			this.Controls.Add(this.buttonSendForReview);
			this.Controls.Add(this.buttonGenFdpToFile);
			this.Controls.Add(this.buttonGenFDP);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CommitedIssueTools";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Commited Issue Tools";
			this.Load += new System.EventHandler(this.CommitedIssueTools_Load);
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcherReviewers)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonGenFDP;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonSendForReview;
		private System.Windows.Forms.Button buttonGenFdpToFile;
		private System.Windows.Forms.LinkLabel linkLabelRegressBuild;
		private System.Windows.Forms.Button buttonComposeReviewRequestHtml;
		private System.Windows.Forms.ComboBox comboBoxReviewer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolTip toolTip;
		private UIControls.SimplifiedBackgroundOperation bgOperationProgress;
		private System.Windows.Forms.Button buttonPostFDP;
		private System.IO.FileSystemWatcher fileSystemWatcherReviewers;
	}
}