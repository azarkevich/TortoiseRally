using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Disposables;
using UIControls.Core;

namespace TrackGearLibrary
{
	public partial class CancellableWait : Form
	{
		public static Task<T> Wait<T>(string waitFor, Task<T> task, int showDlgTimeoutMs = 0, CancellationTokenSource cts = null, bool abandonOnCancel = true)
		{
			// already completed
			if(task.IsCompleted)
				return task;

			if(showDlgTimeoutMs != 0)
			{
				try
				{
					task.Wait(showDlgTimeoutMs);
				}
				catch
				{
					// task still hold error information and client will read it
				}

				// already completed
				if (task.IsCompleted)
					return task;
			}

			var dlg = new CancellableWait(waitFor);

			// dialog closer
			var close = new SingleAssignmentDisposable { Disposable = Disposable.Create(dlg.Close) };

			task.ContinueWith(t => close.Dispose(), TaskScheduler.FromCurrentSynchronizationContext());

			dlg.ShowDialog();

			// if dialog closed not by task completion - cancel it
			if (!task.IsCompleted)
			{
				UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.CancellableWaitCancelled);

				if (cts != null)
					cts.Cancel();

				// do not wait cancellation of original task.
				// if cts == null, then cancellation impossible and abandon is only way
				// if cts != null, then cancellation possible, but client can want abandon
				if (cts == null || abandonOnCancel)
				{
					var ts = new TaskCompletionSource<T>();
					ts.SetCanceled();
					return ts.Task;
				}
			}

			return task;
		}

		CancellableWait(string waitFor)
		{
			InitializeComponent();

			if(Parent == null)
				StartPosition = FormStartPosition.CenterScreen;

			labelWaitFor.Text = waitFor;
		}
	}
}
