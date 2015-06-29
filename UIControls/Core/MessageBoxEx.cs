using System.Windows.Forms;

namespace TrackGearLibrary.Core
{
	public static class MessageBoxEx
	{
		public static void ShowError(string text)
		{
			MessageBox.Show(text, "Rally Tools: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void ShowWarning(string text, IWin32Window owner = null)
		{
			MessageBox.Show(owner, text, "Rally Tools: Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	}
}
