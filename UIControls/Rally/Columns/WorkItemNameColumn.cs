namespace TrackGearLibrary.Rally.Columns
{
	class WorkItemNameColumn : ColumnBase
	{
		public WorkItemNameColumn()
		{
			Header = "Name";
		}

		public override string GetText(Artifact workItem)
		{
			return workItem.Name;
		}
	}
}
