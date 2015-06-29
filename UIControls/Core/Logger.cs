using System;
using System.IO;

namespace UIControls.Core
{
	public static class Logger
	{
		public static void LogIt(string text0)
		{
			try
			{
				var path = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					Constants.AppDataDirName,
					"application.log"
				);

				using (var sw = File.AppendText(path))
				{
					sw.Write("{0:s}: ", DateTimeOffset.Now);
					sw.WriteLine(text0);
				}
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{
			}
		}

		public static void LogIt(string fmt, params object[] prms)
		{
			LogIt(string.Format(fmt, prms));
		}
	}
}
