using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TrackGearLibrary.Rally.Columns
{
	abstract class ColumnBase : IComparer<VirtualListItemArtifact>
	{
		public string Header;
		public int Width;
		public HorizontalAlignment TextAlign = HorizontalAlignment.Left;

		public abstract string GetText(Artifact workItem);

		public virtual Color GetForeColor(Artifact issue, Color def)
		{
			return def;
		}

		public virtual Color GetBackColor(Artifact issue, Color def)
		{
			return def;
		}

		public virtual Font GetFont(Artifact issue, Font def)
		{
			return def;
		}

		public virtual int Compare(VirtualListItemArtifact x, VirtualListItemArtifact y)
		{
			return String.CompareOrdinal(GetText(x.WorkItem), GetText(y.WorkItem));
		}
	}
}
