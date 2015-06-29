using System.Diagnostics;
using System.Globalization;

namespace TrackGearLibrary.Core.MergeMessage
{
	[DebuggerDisplay("Range = ({RangeStart}, {RangeEnd})")]
	struct RevRange
	{
		public long RangeStart;
		public long RangeEnd;

		public override string ToString()
		{
			if (RangeStart == RangeEnd)
				return RangeStart.ToString(CultureInfo.InvariantCulture);

			return string.Format("{0}-{1}", RangeStart, RangeEnd);
		}
	}
}
