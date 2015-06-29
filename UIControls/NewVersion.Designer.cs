namespace TrackGearLibrary
{
	partial class NewVersion
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
			this.label1 = new System.Windows.Forms.Label();
			this.dnlLinkLabel = new System.Windows.Forms.LinkLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkBoxMute = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "New version available:";
			// 
			// dnlLinkLabel
			// 
			this.dnlLinkLabel.AutoSize = true;
			this.dnlLinkLabel.Location = new System.Drawing.Point(132, 9);
			this.dnlLinkLabel.Name = "dnlLinkLabel";
			this.dnlLinkLabel.Size = new System.Drawing.Size(55, 13);
			this.dnlLinkLabel.TabIndex = 1;
			this.dnlLinkLabel.TabStop = true;
			this.dnlLinkLabel.Text = "linkLabel1";
			// 
			// checkBoxMute
			// 
			this.checkBoxMute.AutoSize = true;
			this.checkBoxMute.Location = new System.Drawing.Point(15, 33);
			this.checkBoxMute.Name = "checkBoxMute";
			this.checkBoxMute.Size = new System.Drawing.Size(186, 17);
			this.checkBoxMute.TabIndex = 2;
			this.checkBoxMute.Text = "Do not show again for this version";
			this.checkBoxMute.UseVisualStyleBackColor = true;
			this.checkBoxMute.CheckedChanged += new System.EventHandler(this.checkBoxMute_CheckedChanged);
			// 
			// NewVersion
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(247, 62);
			this.Controls.Add(this.checkBoxMute);
			this.Controls.Add(this.dnlLinkLabel);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "NewVersion";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Version";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel dnlLinkLabel;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkBoxMute;
	}
}