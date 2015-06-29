using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using RallyToolsCore.Properties;
using UIControls.Core;

namespace TrackGearLibrary.Core
{
	public partial class NewVersionControl : UserControl
	{
		public static string ForcedProductVersion;
		public new static string ProductVersion;

		public NewVersionControl()
		{
			InitializeComponent();

			linkLabelNewVersion.Text = "Bugs && Wishes";
			linkLabelNewVersion.LinkClicked += ComposeFeedback;
		}

		void NewVersionControl_Load(object sender, EventArgs e)
		{
			CheckVersion();
		}

		static void ComposeFeedback(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/azarkevich/TortoiseRally/issues");
		}

		class UpdateInformation
		{
			public string Version;
			public string ChangeLog;
			public Uri ChangeLogUri;
			public Uri SetupUri;
			public Uri DownloadUri;
		}

		static readonly Task<UpdateInformation> NewVersionTask;

		static NewVersionControl()
		{
			ProductVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			NewVersionTask = Task.Factory.StartNew(() => {

				var updateManifestUri = new Uri(Settings.Default.UpdateManifestUrl);

				var wc = new WebClient { UseDefaultCredentials = true };
				wc.Headers.Add("X-RallyTools-Version", ProductVersion);

				var manifest = wc.DownloadString(updateManifestUri);

				var xManifest = XDocument.Parse(manifest).Element("manifest");

				if (xManifest == null)
					return null;

				var xVersion = xManifest.Element("version");

				if (xVersion == null || string.IsNullOrWhiteSpace(xVersion.Value))
					return null;

				var currentVersion = new SoftwareVersion.SoftwareVersion(ForcedProductVersion ?? ProductVersion);
				var latestVersion = new SoftwareVersion.SoftwareVersion(xVersion.Value);

				if (currentVersion.CompareTo(latestVersion) >= 0)
					return null;

				var ret = new UpdateInformation
				{
					Version = xVersion.Value
				};

				var xSetupUri = xManifest.Element("setup");
				ret.SetupUri = new Uri(updateManifestUri, new Uri(xSetupUri != null ? xSetupUri.Value : "Setup.msi", UriKind.RelativeOrAbsolute));

				var xChangelogUri = xManifest.Element("changelog");
				if (xChangelogUri != null)
				{
					ret.ChangeLogUri = new Uri(updateManifestUri, new Uri(xChangelogUri.Value, UriKind.RelativeOrAbsolute));

					// download changelog
					try
					{
						ret.ChangeLog = wc.DownloadString(ret.ChangeLogUri);
					}
					catch(Exception ex)
					{
						Logger.LogIt(ex.ToString());
						ret.ChangeLog = "Error: " + ex;
					}
				}

				var xDownloadUri = xManifest.Element("download");
				if (xDownloadUri != null)
					ret.DownloadUri = new Uri(updateManifestUri, new Uri(xDownloadUri.Value, UriKind.RelativeOrAbsolute));
				else
					ret.DownloadUri = ret.SetupUri;

				return ret;
			});
		}

		void CheckVersion()
		{
			NewVersionTask
				.ContinueWith(t => {

				var updateInfo = t.Result;

				if (updateInfo == null)
					return;

				linkLabelNewVersion.Invoke((Action)delegate {
					const string prefix = "New version available: ";
					linkLabelNewVersion.Text = prefix + updateInfo.Version;
					linkLabelNewVersion.Links.Add(prefix.Length, linkLabelNewVersion.Text.Length - prefix.Length, null);
					linkLabelNewVersion.LinkClicked -= ComposeFeedback;
					linkLabelNewVersion.LinkClicked += (s, e) => Process.Start(t.Result.DownloadUri.OriginalString);
				});

				var changeLines = updateInfo.ChangeLog.Replace("\r\n", "\n").Replace("\n\r", "\n").Replace("\r", "\n").Split('\n');

				try
				{
					var currentVersion = new SoftwareVersion.SoftwareVersion(ForcedProductVersion ?? ProductVersion);
					var verRx = @"^\s*" + string.Join("\\.", currentVersion.VersionComponents.Take(2).Select(c => c.Component)) + @"\s*$";
					var rx = new Regex(verRx);

					changeLines = changeLines.TakeWhile(l => !rx.IsMatch(l)).ToArray();
				}
				catch(Exception ex)
				{
					Logger.LogIt(ex.ToString());
				}

				if (changeLines.Length > 50)
					changeLines = changeLines.Take(20).Concat(new [] { "\n", "  ..." }).ToArray();

				linkLabelNewVersion.Invoke((Action)(() => {
					toolTip.SetToolTip(linkLabelNewVersion, string.Join("\n", changeLines));

					if (!string.IsNullOrWhiteSpace(Settings.Default.MutedVersion))
					{
						var latestVersion = new SoftwareVersion.SoftwareVersion(updateInfo.Version);
						var mutedVersion = new SoftwareVersion.SoftwareVersion(Settings.Default.MutedVersion);
						if (mutedVersion.CompareTo(latestVersion) >= 0)
							return;
					}

					var dlg = new NewVersion(updateInfo.Version, t.Result.DownloadUri.OriginalString, changeLines);

					dlg.OnMute += mute => {
						Settings.Default.MutedVersion = mute ? updateInfo.Version : "";
						Settings.Default.Save();
					};
					dlg.Show(this);

				}));
			}, TaskContinuationOptions.OnlyOnRanToCompletion);
		}
	}
}
