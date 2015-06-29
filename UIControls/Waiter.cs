using System;
using System.Windows.Forms;

namespace RallyToolsCore
{
	class Waiter : IDisposable
	{
		readonly Control _waitOn;
		readonly Cursor _old;
		public Waiter(Control waitOn)
		{
			_waitOn = waitOn;
			_old = _waitOn.Cursor;
			_waitOn.Cursor = Cursors.WaitCursor;
			_waitOn.Enabled = false;
		}

		public void Dispose()
		{
			_waitOn.Cursor = _old;
			_waitOn.Enabled = true;
		}
	}
}
