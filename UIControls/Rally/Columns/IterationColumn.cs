namespace TrackGearLibrary.Rally.Columns
{
	class IterationColumn : ColumnBase
	{
		public IterationColumn()
		{
			Header = "Iteration";
		}

		public override string GetText(Artifact workItem)
		{
			return workItem.Iteration.Name ?? "<N/A>";
		}
	}
}
