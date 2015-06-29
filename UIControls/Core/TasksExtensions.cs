using System.Threading;
using System.Threading.Tasks;

namespace TrackGearLibrary.Core
{
	public static class TasksExtensions
	{
		public static Task<T> WaitDlg<T>(this Task<T> task, string waitMsg, int showDlgTimeoutMs = 0, CancellationTokenSource cts = null, bool abandonOnCancel = true)
		{
			return CancellableWait.Wait(waitMsg, task, showDlgTimeoutMs, cts, abandonOnCancel);
		}
	}
}
