using System.Windows.Forms;
using TrackGearLibrary.Rally.Columns;

namespace TrackGearLibrary.Rally
{
	class VirtualListItemArtifact
	{
		public readonly Artifact WorkItem;
		readonly ColumnsCollection _columns;

		public VirtualListItemArtifact(Artifact workItem, ColumnsCollection columns)
		{
			WorkItem = workItem;
			_columns = columns;
		}

		public ListViewItem ListItem
		{
			get
			{
				if (_cachedListItem == null)
				{
					_cachedListItem = CreateListItem();
				}
				return _cachedListItem;
			}
		}
		ListViewItem _cachedListItem;

		ListViewItem CreateListItem()
		{
			var lvi = new ListViewItem { UseItemStyleForSubItems = false };

			if (_columns.Columns.Length > 0)
			{
				var first = _columns.Columns[0];
				lvi.Text = first.GetText(WorkItem);
				lvi.ForeColor = first.GetForeColor(WorkItem, lvi.ForeColor);
				lvi.BackColor = first.GetBackColor(WorkItem, lvi.BackColor);
				lvi.Font = first.GetFont(WorkItem, lvi.Font);
				//lvi.ImageKey
				//lvi.IndentCount
				//lvi.StateImageIndex
				//lvi.Tag = 
				//lvi.ToolTipText

				for (int i = 1; i < _columns.Columns.Length; i++)
				{
					var column = _columns.Columns[i];
					var lvis = new ListViewItem.ListViewSubItem(lvi, column.GetText(WorkItem));
					lvis.ForeColor = column.GetForeColor(WorkItem, lvis.ForeColor);
					lvis.BackColor = column.GetBackColor(WorkItem, lvis.BackColor);
					lvis.Font = column.GetFont(WorkItem, lvis.Font);

					lvi.SubItems.Add(lvis);
				}
			}

			return lvi;
		}
	}
}
