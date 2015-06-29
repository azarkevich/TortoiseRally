namespace TrackGearLibrary.Core
{
	partial class NewVersionControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.linkLabelNewVersion = new System.Windows.Forms.LinkLabel();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// linkLabelNewVersion
			// 
			this.linkLabelNewVersion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.linkLabelNewVersion.ForeColor = System.Drawing.Color.Red;
			this.linkLabelNewVersion.Location = new System.Drawing.Point(0, 0);
			this.linkLabelNewVersion.Name = "linkLabelNewVersion";
			this.linkLabelNewVersion.Size = new System.Drawing.Size(276, 17);
			this.linkLabelNewVersion.TabIndex = 6;
			this.linkLabelNewVersion.TabStop = true;
			this.linkLabelNewVersion.Text = "new version";
			this.linkLabelNewVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// NewVersionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.linkLabelNewVersion);
			this.Name = "NewVersionControl";
			this.Size = new System.Drawing.Size(276, 17);
			this.Load += new System.EventHandler(this.NewVersionControl_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.LinkLabel linkLabelNewVersion;
		private System.Windows.Forms.ToolTip toolTip;
	}
}
