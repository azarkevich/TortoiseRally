using System.IO;

namespace TrackGearLibrary.Tools
{
	public static class SvnUtils
	{
		public static string FindSvnWC(string dir)
		{
			if (Directory.Exists(Path.Combine(dir, ".svn")))
				return dir;

			var topdir = Path.GetDirectoryName(dir);

			if (topdir == null || dir == topdir)
				return null;

			return FindSvnWC(topdir);
		}
	}
}
