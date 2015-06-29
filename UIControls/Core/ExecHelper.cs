using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace TrackGearLibrary
{
	public class ExecHelper
	{
		public string WorkingDirectory;
		public string ExePath;

		public ExecHelper(string exe, string workingDir = null)
		{
			WorkingDirectory = workingDir ?? Environment.CurrentDirectory;
			ExePath = exe;
		}

		public IList<string> ExecuteForLines(params string[] arguments)
		{
			var lines = new List<string>();

			Execute(lines.Add, arguments);

			return lines;
		}

		public void Execute(Action<string> aggregate, params string[] arguments)
		{
			var p = new Process();
			p.StartInfo.FileName = ExePath;
			p.StartInfo.Arguments = BuildArguments(arguments);

			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.WorkingDirectory = WorkingDirectory;
			p.StartInfo.CreateNoWindow = true;

			p.EnableRaisingEvents = true;

			p.OutputDataReceived += (s, e) => aggregate(e.Data ?? "");

			var sbErr = new StringBuilder();
			p.ErrorDataReceived += (s, e) => sbErr.AppendLine(e.Data ?? "");

			try{
				p.Start();
			}
			catch(Exception ex)
			{
				MessageBox.Show(string.Format("Can't execute '{0}':\n{1}", ExePath, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			p.BeginErrorReadLine();
			p.BeginOutputReadLine();

			p.WaitForExit();

			if (p.ExitCode != 0)
				throw new ExecException(ExePath, p.StartInfo.Arguments, p.ExitCode, sbErr.ToString());
		}

		public string Execute(params string[] arguments)
		{
			var sbOut = new StringBuilder();
			try
			{
				Execute(l => sbOut.AppendLine(l), arguments);
				return sbOut.ToString();
			}
			catch (ExecException ex)
			{
				ex.StdOut = sbOut.ToString().Trim();
				throw;
			}
		}

		public static string BuildArguments(IEnumerable<string> arguments)
		{
			var escaped = arguments
				.Select(EscapeArgument)
				.ToArray()
			;

			return string.Join(" ", escaped);
		}

		public static string EscapeArgument(string arg)
		{
			if (!arg.Contains(' '))
				return arg;
			
			return "\"" + arg.Replace("\"", "\\\"") + "\"";
		}
	}
}
