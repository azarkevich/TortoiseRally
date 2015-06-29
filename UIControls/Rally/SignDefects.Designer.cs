namespace TrackGearLibrary.Rally
{
	partial class SignDefects
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignDefects));
			this.backgroundOperation = new UIControls.BackgroundOperation();
			this.listViewArtifacts = new System.Windows.Forms.ListView();
			this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colArtifactName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colParentName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.simplifiedBackgroundOperation = new UIControls.SimplifiedBackgroundOperation();
			this.colSigned = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.buttonDoSign = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// backgroundOperation
			// 
			this.backgroundOperation.AbandonOnCancel = true;
			this.backgroundOperation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.backgroundOperation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.backgroundOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.backgroundOperation.CloseOnRanToCompletion = true;
			this.backgroundOperation.Location = new System.Drawing.Point(320, 150);
			this.backgroundOperation.Name = "backgroundOperation";
			this.backgroundOperation.Size = new System.Drawing.Size(260, 93);
			this.backgroundOperation.TabIndex = 0;
			this.backgroundOperation.Visible = false;
			this.backgroundOperation.WaitMessage = "Please wait ...";
			// 
			// listViewArtifacts
			// 
			this.listViewArtifacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewArtifacts.CheckBoxes = true;
			this.listViewArtifacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colArtifactName,
            this.colParentName,
            this.colSigned});
			this.listViewArtifacts.FullRowSelect = true;
			this.listViewArtifacts.GridLines = true;
			this.listViewArtifacts.HideSelection = false;
			this.listViewArtifacts.Location = new System.Drawing.Point(12, 12);
			this.listViewArtifacts.Name = "listViewArtifacts";
			this.listViewArtifacts.Size = new System.Drawing.Size(872, 355);
			this.listViewArtifacts.TabIndex = 1;
			this.listViewArtifacts.UseCompatibleStateImageBehavior = false;
			this.listViewArtifacts.View = System.Windows.Forms.View.Details;
			// 
			// colId
			// 
			this.colId.Text = "Formatted Id";
			this.colId.Width = 98;
			// 
			// colArtifactName
			// 
			this.colArtifactName.Text = "Artifact Name";
			this.colArtifactName.Width = 398;
			// 
			// colParentName
			// 
			this.colParentName.Text = "Parent Name";
			this.colParentName.Width = 307;
			// 
			// simplifiedBackgroundOperation
			// 
			this.simplifiedBackgroundOperation.AutoSize = true;
			this.simplifiedBackgroundOperation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.simplifiedBackgroundOperation.Location = new System.Drawing.Point(12, 375);
			this.simplifiedBackgroundOperation.Name = "simplifiedBackgroundOperation";
			this.simplifiedBackgroundOperation.Size = new System.Drawing.Size(63, 21);
			this.simplifiedBackgroundOperation.TabIndex = 2;
			// 
			// colSigned
			// 
			this.colSigned.Text = "Signed";
			this.colSigned.Width = 41;
			// 
			// buttonDoSign
			// 
			this.buttonDoSign.Enabled = false;
			this.buttonDoSign.Location = new System.Drawing.Point(809, 375);
			this.buttonDoSign.Name = "buttonDoSign";
			this.buttonDoSign.Size = new System.Drawing.Size(75, 23);
			this.buttonDoSign.TabIndex = 3;
			this.buttonDoSign.Text = "Sign";
			this.buttonDoSign.UseVisualStyleBackColor = true;
			this.buttonDoSign.Click += new System.EventHandler(this.buttonDoSign_Click);
			// 
			// SignDefects
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(896, 408);
			this.Controls.Add(this.buttonDoSign);
			this.Controls.Add(this.simplifiedBackgroundOperation);
			this.Controls.Add(this.backgroundOperation);
			this.Controls.Add(this.listViewArtifacts);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SignDefects";
			this.Text = "Sign Defects";
			this.Load += new System.EventHandler(this.SignDefects_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UIControls.BackgroundOperation backgroundOperation;
		private System.Windows.Forms.ListView listViewArtifacts;
		private System.Windows.Forms.ColumnHeader colId;
		private System.Windows.Forms.ColumnHeader colArtifactName;
		private System.Windows.Forms.ColumnHeader colParentName;
		private UIControls.SimplifiedBackgroundOperation simplifiedBackgroundOperation;
		private System.Windows.Forms.ColumnHeader colSigned;
		private System.Windows.Forms.Button buttonDoSign;
	}
}