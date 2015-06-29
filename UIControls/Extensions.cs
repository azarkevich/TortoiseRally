using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIControls
{
	public static class Extensions
	{
		public static Task TrackProgress(this Task task, SimplifiedBackgroundOperation op, string message)
		{
			op.Track(task, message);
			return task;
		}

		public static Task<T> TrackProgress<T>(this Task<T> task, SimplifiedBackgroundOperation op, string message)
		{
			op.Track(task, message);
			return task;
		}

		public static Task<T> LockControls<T>(this Task<T> task, params Control[] lockControls)
		{
			var lockControlsList = lockControls.ToList();

			// disable controls
			lockControlsList.ForEach(c => c.Enabled = false);

			// enable controls
			return task
				.ContinueWith(t => { lockControlsList.ForEach(c => c.Enabled = true); return t; }, TaskScheduler.FromCurrentSynchronizationContext())
				.Unwrap()
			;
		}

		/// <summary>
		/// Dispose objects witout guarantie, that this completes before retuned task will be completed
		/// </summary>
		public static Task<T> WithDisposeWeak<T>(this Task<T> task, params IDisposable[] disposables)
		{
			task
				.ContinueWith(t => {

					foreach (var disposable in disposables)
					{
						try
						{
							disposable.Dispose();
						}
						catch
						{
						}
					}

				}, TaskContinuationOptions.ExecuteSynchronously)
			;

			return task;
		}

		public static Task WithDisposeWeak(this Task task, params IDisposable[] disposables)
		{
			// start tracking
			return task
				.ContinueWith(t => (object)null, TaskContinuationOptions.ExecuteSynchronously)
				.WithDisposeWeak(disposables)
			;
		}

		public static Task TrackProgress(this Task task, BackgroundOperation op)
		{
			// start tracking
			var typedTask = task
				.ContinueWith(t => (object)null, TaskContinuationOptions.ExecuteSynchronously)
			;

			return op.TrackWithCancellation(typedTask);
		}

		public static Task<T> TrackProgress<T>(this Task<T> task, BackgroundOperation op, CancellationTokenSource cts = null)
		{
			// start tracking
			return op.TrackWithCancellation(task);
		}
	}
}
