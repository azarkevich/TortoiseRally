using System.Collections.Generic;
using System.Windows.Forms;

namespace TrackGearLibrary.Rally.Columns
{
	class ColumnsCollection : IComparer<VirtualListItemArtifact>
	{
		public ColumnBase[] Columns;

		public int SortColumn = -1;
		public SortOrder SortOrder = SortOrder.None;

		public int PrevSortColumn = -1;
		public SortOrder PrevSortOrder = SortOrder.None;

		public int Compare(VirtualListItemArtifact x, VirtualListItemArtifact y)
		{
			var cmp = Columns[SortColumn].Compare(x, y);

			if (SortOrder == SortOrder.Descending)
				cmp = -cmp;

			if (cmp == 0 && PrevSortColumn != -1)
			{
				cmp = Columns[PrevSortColumn].Compare(x, y);
				if (PrevSortOrder == SortOrder.Descending)
					cmp = -cmp;
			}

			// normalize
			if (cmp < -1)
				cmp = -1;
			else if (cmp > 1)
				cmp = 1;

			return cmp;
		}
	}
}
