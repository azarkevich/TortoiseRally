using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using RallyToolsCore.Properties;

namespace UIControls
{
	public partial class SimplifiedBackgroundOperation : UserControl
	{
		public SimplifiedBackgroundOperation()
		{
			InitializeComponent();
		}

		[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return label.Text;
			}
			set
			{
				label.Text = value;
				_rantoCompletionText = value;
			}
		}
		string _rantoCompletionText;

		public void Track(Task task, string inprogressText)
		{
			pictureBox.Image = Resources.spin;
			label.Text = inprogressText;
			label.ForeColor = DefaultForeColor;
			toolTip.RemoveAll();

			task
				.ContinueWith(t => {

					if (t.IsCanceled)
					{
						label.Text = "Cancelled";
						label.ForeColor = Color.Peru;
						pictureBox.Image = Resources.Ok;
					}
					else if (t.IsFaulted)
					{
						pictureBox.Image = Resources.Error;
						label.Text = "ERROR";
						label.ForeColor = Color.Red;
						toolTip.SetToolTip(pictureBox, t.Exception.InnerException.Message);
						toolTip.SetToolTip(label, t.Exception.InnerException.ToString());
					}
					else
					{
						pictureBox.Image = Resources.Ok;
						label.Text = _rantoCompletionText;
					}

				}, TaskScheduler.FromCurrentSynchronizationContext())
			;
		}
	}
}
