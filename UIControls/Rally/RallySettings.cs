using System;
using System.DirectoryServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RallyToolsCore.Properties;
using UIControls.Core;

namespace TrackGearLibrary.Rally
{
	public partial class RallySettings : Form
	{
		string _oldUserName;
		string _oldUserPassword;

		public RallySettings()
		{
			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.RallySettings);

			_oldUserName = Settings.Default.RallyUser;
			_oldUserPassword = Settings.Default.RallyPassword;

			InitializeComponent();
		}

		void RallySettings_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textBoxUserName.Text))
			{
				var self = new DirectorySearcher(string.Format("(|(cn={0})(sAMAccountName={0}))", Environment.UserName), new[] { "mail" }).FindOne();
				if (self != null)
				{
					textBoxUserName.Text = self.Properties["mail"][0].ToString();
				}
			}
		}

		void buttonOK_Click(object sender, EventArgs e)
		{
			if (Settings.Default.RallyUser != _oldUserName || Settings.Default.RallyPassword != _oldUserPassword)
			{
				try
				{
					//Settings.Default.RallyToken = RetrieveApiKey();

					_oldUserName = Settings.Default.RallyUser;
					_oldUserPassword = Settings.Default.RallyPassword;
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					DialogResult = DialogResult.None;
					return;
				}
			}
			Settings.Default.Save();
		}

		void buttonCancel_Click(object sender, EventArgs e)
		{
			Settings.Default.Reload();
		}

		void textBoxNoiseTasksRegex_TextChanged(object sender, EventArgs e)
		{
			try
			{
				new Regex(textBoxNoiseTasksRegex.Text);
				textBoxNoiseTasksRegex.BackColor = Color.LightGreen;
			}
			catch (Exception)
			{
				textBoxNoiseTasksRegex.BackColor = Color.LightPink;
			}
		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}
	}
}
