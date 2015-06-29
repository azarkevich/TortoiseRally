using System;

namespace TrackGearLibrary
{
	class ExecException : ApplicationException
	{
		public string ExePath;
		public string Arguments;
		public int ExitCode;
		public string StdErr;
		public string StdOut;

		public ExecException(string exePath, string arguments, int exitCode, string stdErr)
		{
			ExePath = exePath;
			Arguments = arguments;
			ExitCode = exitCode;
			StdErr = stdErr.Trim();
		}
	}
}
