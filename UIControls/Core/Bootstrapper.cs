using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.Win32;
using RallyToolsCore.Properties;
using UIControls.Core;

namespace TrackGearLibrary.Core
{
	public static class Bootstrapper
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Init(bool resetSettings)
		{
			if (resetSettings)
			{
				Settings.Default.Reset();
				Settings.Default.Save();
			}

			_thisAssembly = Assembly.GetExecutingAssembly();
			_baseDir = Path.GetDirectoryName(_thisAssembly.Location);
			Debug.Assert(_baseDir != null);

			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

			// allow all SSL certificates
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
		}

		static string _installedTSvnVersion;
		static string _baseDir;
		static Assembly _thisAssembly;

		static string GetInstalledTSvnVersion()
		{
			if (_installedTSvnVersion == null)
			{
				var key = Registry.CurrentUser.OpenSubKey(@"Software\TortoiseSVN");
				if (key != null)
				{
					var cv = key.GetValue("CurrentVersion");
					if(cv != null && cv.ToString().StartsWith("1.8."))
						_installedTSvnVersion = "1.8";
					else
						_installedTSvnVersion = "1.7";
				}
				else
				{
					_installedTSvnVersion = "1.8";
				}
			}
			return _installedTSvnVersion;
		}

		static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("SharpSvn"))
			{
				var bitness = Environment.Is64BitProcess ? "x64" : "x86";
				var version = GetInstalledTSvnVersion();

				var sharpSvnFile = string.Format("SharpSvn.{0}-{1}.dll", version, bitness);

				var sharpSvnPath = Path.Combine(_baseDir, sharpSvnFile);

				if (!File.Exists(sharpSvnPath))
				{
					MessageBox.Show("Can't find '" + sharpSvnFile + "'\nin\n" + _baseDir, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return null;
				}

				return Assembly.LoadFile(sharpSvnPath);
			}

			// issue with Load Context (?) for native host process (TortoiseProc.exe)
			// Type.GetType in System.dll!System.Configuration.ApplicationSettingsBase.CreateSetting failed, because TrackGearLibrary failed to load
			if (args.Name == _thisAssembly.FullName)
			{
				return _thisAssembly;
			}

			// known case
			if (args.Name.StartsWith("TrackGearLibrary.resources") || args.Name.StartsWith("UIControls.resources"))
				return null;

			Logger.LogIt("OnAssemblyResolve failed for: {0}", args.Name);

			return null;
		}
	}
}
