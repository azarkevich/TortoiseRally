using System.Xml;

namespace TrackGearLibrary
{
	class SvnWrapper
	{
		readonly ExecHelper _exec;
		
		public SvnWrapper()
		{
			_exec = new ExecHelper("svn.exe");
		}

		public XmlDocument GetLogEntry(string path, string rev = "HEAD")
		{
			var res = _exec.Execute("log", path, "-r", rev, "--xml", "--verbose");

			var doc = new XmlDocument();
			doc.LoadXml(res);

			return doc;
		}
	}
}
