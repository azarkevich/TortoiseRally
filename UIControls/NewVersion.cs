using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TrackGearLibrary
{
	public partial class NewVersion : Form
	{
		public delegate void MuteVersion(bool mute);

		public event MuteVersion OnMute;

		public NewVersion(string newVersion, string dnlUri, string[] changeLines)
		{
			InitializeComponent();

			dnlLinkLabel.Text = newVersion;
			dnlLinkLabel.LinkArea = new LinkArea(0, dnlLinkLabel.Text.Length);
			dnlLinkLabel.LinkClicked += (sender, args) => Process.Start(dnlUri);

			toolTip1.SetToolTip(dnlLinkLabel, string.Join("\n", changeLines));
		}

		void checkBoxMute_CheckedChanged(object sender, EventArgs e)
		{
			if (OnMute != null)
				OnMute(checkBoxMute.Checked);
		}
	}
}
