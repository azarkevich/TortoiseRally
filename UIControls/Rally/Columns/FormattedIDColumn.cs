using System.Drawing;

namespace TrackGearLibrary.Rally.Columns
{
	class FormattedIDColumn : ColumnBase
	{
		public FormattedIDColumn()
		{
			Header = "#";
		}

		public override string GetText(Artifact workItem)
		{
			return workItem.FormattedID;
		}

		public override Color GetBackColor(Artifact issue, Color def)
		{
			if (issue.Type == ArtifactType.Defect)
				return Color.LightCoral;

			if (issue.Type == ArtifactType.Task)
			{
				if(issue.Parent.Type == ArtifactType.Defect)
					return Color.MistyRose;

				return Color.LightGreen;
			}

			return def;
		}
	}
}
