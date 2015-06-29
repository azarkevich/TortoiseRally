namespace TrackGearLibrary.Svn
{
	partial class SvnLog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SvnLog));
			this.listViewLog = new System.Windows.Forms.ListView();
			this.colRevision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.generateReviewRequestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxUrl = new System.Windows.Forms.TextBox();
			this.buttonLoad = new System.Windows.Forms.Button();
			this.buttonLoadNext = new System.Windows.Forms.Button();
			this.textBoxLoadBunch = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.loadingOperation = new UIControls.BackgroundOperation();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// listViewLog
			// 
			this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRevision,
            this.colAuthor,
            this.colMessage});
			this.listViewLog.ContextMenuStrip = this.contextMenu;
			this.listViewLog.FullRowSelect = true;
			this.listViewLog.GridLines = true;
			this.listViewLog.Location = new System.Drawing.Point(12, 32);
			this.listViewLog.Name = "listViewLog";
			this.listViewLog.Size = new System.Drawing.Size(1257, 459);
			this.listViewLog.TabIndex = 0;
			this.listViewLog.UseCompatibleStateImageBehavior = false;
			this.listViewLog.View = System.Windows.Forms.View.Details;
			// 
			// colRevision
			// 
			this.colRevision.Text = "#";
			this.colRevision.Width = 72;
			// 
			// colAuthor
			// 
			this.colAuthor.Text = "Author";
			this.colAuthor.Width = 155;
			// 
			// colMessage
			// 
			this.colMessage.Text = "Message";
			this.colMessage.Width = 986;
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateReviewRequestToolStripMenuItem});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(210, 26);
			// 
			// generateReviewRequestToolStripMenuItem
			// 
			this.generateReviewRequestToolStripMenuItem.Name = "generateReviewRequestToolStripMenuItem";
			this.generateReviewRequestToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.generateReviewRequestToolStripMenuItem.Text = "Generate review request...";
			this.generateReviewRequestToolStripMenuItem.Click += new System.EventHandler(this.generateReviewRequestToolStripMenuItem_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Url:";
			// 
			// textBoxUrl
			// 
			this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxUrl.Location = new System.Drawing.Point(41, 6);
			this.textBoxUrl.Name = "textBoxUrl";
			this.textBoxUrl.Size = new System.Drawing.Size(925, 20);
			this.textBoxUrl.TabIndex = 2;
			this.textBoxUrl.TextChanged += new System.EventHandler(this.textBoxUrl_TextChanged);
			// 
			// buttonLoad
			// 
			this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLoad.Location = new System.Drawing.Point(972, 4);
			this.buttonLoad.Name = "buttonLoad";
			this.buttonLoad.Size = new System.Drawing.Size(75, 23);
			this.buttonLoad.TabIndex = 3;
			this.buttonLoad.Text = "Load";
			this.buttonLoad.UseVisualStyleBackColor = true;
			this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
			// 
			// buttonLoadNext
			// 
			this.buttonLoadNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLoadNext.Location = new System.Drawing.Point(1053, 4);
			this.buttonLoadNext.Name = "buttonLoadNext";
			this.buttonLoadNext.Size = new System.Drawing.Size(86, 23);
			this.buttonLoadNext.TabIndex = 3;
			this.buttonLoadNext.Text = "Load Next:";
			this.buttonLoadNext.UseVisualStyleBackColor = true;
			this.buttonLoadNext.Click += new System.EventHandler(this.buttonLoadNext_Click);
			// 
			// textBoxLoadBunch
			// 
			this.textBoxLoadBunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLoadBunch.Location = new System.Drawing.Point(1145, 6);
			this.textBoxLoadBunch.Name = "textBoxLoadBunch";
			this.textBoxLoadBunch.Size = new System.Drawing.Size(64, 20);
			this.textBoxLoadBunch.TabIndex = 4;
			this.textBoxLoadBunch.Text = "100";
			this.textBoxLoadBunch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(1215, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "messages";
			// 
			// loadingOperation
			// 
			this.loadingOperation.AbandonOnCancel = true;
			this.loadingOperation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.loadingOperation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.loadingOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.loadingOperation.CloseOnRanToCompletion = true;
			this.loadingOperation.Location = new System.Drawing.Point(522, 183);
			this.loadingOperation.Name = "loadingOperation";
			this.loadingOperation.Size = new System.Drawing.Size(247, 117);
			this.loadingOperation.TabIndex = 6;
			this.loadingOperation.Visible = false;
			this.loadingOperation.WaitMessage = "Load revisions...";
			// 
			// SvnLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1281, 503);
			this.Controls.Add(this.loadingOperation);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxLoadBunch);
			this.Controls.Add(this.buttonLoadNext);
			this.Controls.Add(this.buttonLoad);
			this.Controls.Add(this.textBoxUrl);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listViewLog);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SvnLog";
			this.Text = "Svn Log";
			this.Load += new System.EventHandler(this.SvnLog_Load);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listViewLog;
		private System.Windows.Forms.ColumnHeader colRevision;
		private System.Windows.Forms.ColumnHeader colAuthor;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxUrl;
		private System.Windows.Forms.Button buttonLoad;
		private System.Windows.Forms.Button buttonLoadNext;
		private System.Windows.Forms.TextBox textBoxLoadBunch;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem generateReviewRequestToolStripMenuItem;
		private UIControls.BackgroundOperation loadingOperation;
	}
}