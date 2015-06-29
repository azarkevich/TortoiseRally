namespace TrackGearLibrary.Rally.Columns
{
	class ParentColumn : ColumnBase
	{
		public ParentColumn()
		{
			Header = "Parent";
		}

		public override string GetText(Artifact workItem)
		{
			return workItem.Parent.Name ?? "<N/A>";
		}
	}
}
