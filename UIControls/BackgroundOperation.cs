using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RallyToolsCore.Properties;
using UIControls.Core;

namespace UIControls
{
	public partial class BackgroundOperation : UserControl
	{
		readonly SynchronizationContext _synchronizationContext;

		public BackgroundOperation()
		{
			_synchronizationContext = SynchronizationContext.Current;

			InitializeComponent();

			WaitMessage = "Please wait ...";
			AbandonOnCancel = true;
			CloseOnRanToCompletion = true;
		}

		[Browsable(true), EditorBrowsable(EditorBrowsableState.Advanced)]
		public string WaitMessage
		{
			get
			{
				return label.Text;
			}
			set
			{
				_originalWaitMessage = value;
				label.Text = value;
			}
		}
		string _originalWaitMessage;

		[Browsable(true)]
		public bool AbandonOnCancel { get; set; }

		[Browsable(true)]
		public bool CloseOnRanToCompletion { get; set; }

		CancellationTokenSource _currentTrackedTaskCancellationSource;
		bool _tcsOwnership;

		bool _hideByClickOnCancel;

		public Task<T> TrackWithCancellation<T>(Task<T> track, CancellationTokenSource cts = null)
		{
			_currentTrackedTaskCancellationSource = cts ?? new CancellationTokenSource();
			_tcsOwnership = (cts == null);

			Visible = true;
			pictureBox.Image = Resources.spin;
			label.ForeColor = DefaultForeColor;
			label.Text = _originalWaitMessage;
			toolTip.RemoveAll();
			_hideByClickOnCancel = false;
			buttonCancel.Text = "Cancel";
			buttonCancel.Enabled = true;

			var task = track
				.ContinueWith(t => {

					if (_tcsOwnership)
					{
						_currentTrackedTaskCancellationSource.Dispose();
					}

					_currentTrackedTaskCancellationSource = null;

					if (t.Status == TaskStatus.RanToCompletion)
					{
						if (CloseOnRanToCompletion)
						{
							Visible = false;
						}
						else
						{
							pictureBox.Image = Resources.Ok;
							label.ForeColor = Color.Green;
							label.Text = "OK";
							buttonCancel.Enabled = true;
							buttonCancel.Text = "Close";
							_hideByClickOnCancel = true;
						}
					}
					else if (t.Status == TaskStatus.Faulted)
					{
						var ex = t.Exception.InnerException;
						pictureBox.Image = Resources.Error;
						toolTip.SetToolTip(pictureBox, ex.ToString());
						toolTip.SetToolTip(label, ex.ToString());

						label.ForeColor = Color.Red;
						label.Text = "Error: " + ex.Message;

						buttonCancel.Enabled = true;
						buttonCancel.Text = "Close";
						_hideByClickOnCancel = true;
					}
					else
					{
						Visible = false;
					}

					return t;

				}, TaskScheduler.FromCurrentSynchronizationContext())
				.Unwrap()
			;

			// if abandon on cancellation is not required - just return task
			if (!AbandonOnCancel)
				return task;

			var tcs = new TaskCompletionSource<T>();

			var reg = _currentTrackedTaskCancellationSource.Token.Register(() => tcs.TrySetCanceled());

			task
				.ContinueWith(t => {
					if (t.IsCanceled)
						tcs.TrySetCanceled();
					else if (t.IsFaulted)
						tcs.TrySetException(t.Exception);
					else
						tcs.TrySetResult(t.Result);
				}, TaskContinuationOptions.ExecuteSynchronously)
				.WithDisposeWeak(reg)
			;

			return tcs.Task;
		}

		void buttonCancel_Click(object sender, System.EventArgs e)
		{
			if (_hideByClickOnCancel)
			{
				Visible = false;
				return;
			}

			UsageMetrics.IncrementUsage(UsageMetrics.UsageKind.BGOperationCancelled);

			var cts = _currentTrackedTaskCancellationSource;
			if (cts != null)
			{
				cts.Cancel();
			}

			// if we not wait till task really cancelled - just hide control
			if (AbandonOnCancel)
			{
				Visible = false;
				return;
			}

			// otherwise - show message 'Cancelling'
			label.Text = "Cancelling ...";
			label.ForeColor = Color.Goldenrod;

			buttonCancel.Enabled = false;
		}

		public void SetProgress(string text)
		{
			_synchronizationContext.Post(_ => { label.Text = text; }, null);
		}
	}
}
