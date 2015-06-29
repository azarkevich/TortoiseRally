using System;
using System.Collections.Generic;
using System.Linq;

namespace TrackGearLibrary.Core.MergeMessage
{
	class RevRangeSet
	{
		readonly RevRange[] _ranges;
		readonly bool _normalized;

		public IEnumerable<RevRange> Ranges
		{
			get
			{
				return _ranges;
			}
		}

		public RevRangeSet(IEnumerable<RevRange> ranges, bool normalzied = false)
		{
			_ranges = ranges.ToArray();
			_normalized = normalzied || _ranges.Length <= 1;
		}

		public override string ToString()
		{
			return string.Join(", ", _ranges.Select(r => r.ToString()).ToArray());
		}

		public RevRangeSet Normalize()
		{
			if (_normalized)
				return new RevRangeSet(_ranges, true);

			var ordered = _ranges.OrderBy(r => r.RangeStart).ToArray();

			var norm = new List<RevRange>();

			var accumRange = ordered[0];

			for (var i = 1; i < ordered.Length; i++)
			{
				// check if current range can be joined to accum
				if (ordered[i].RangeStart <= (accumRange.RangeEnd + 1))
				{
					accumRange.RangeEnd = Math.Max(accumRange.RangeEnd, ordered[i].RangeEnd);
					continue;
				}
				norm.Add(accumRange);
				accumRange = ordered[i];
			}
			norm.Add(accumRange);

			return new RevRangeSet(norm, true);
		}

		public RevRangeSet Subtract(RevRangeSet what)
		{
			if (!_normalized)
				return Normalize().Subtract(what);

			if (!what._normalized)
				return Subtract(what.Normalize());

			// make copies
			var subtracted = _ranges.ToList();

			foreach (var subRange in what._ranges)
			{
				for (var i = 0; i < subtracted.Count; i++)
				{
					// complete cut
					if (subRange.RangeStart <= subtracted[i].RangeStart && subRange.RangeEnd >= subtracted[i].RangeEnd)
					{
						subtracted.RemoveAt(i);
						// can eat other ranges
						i--;
						continue;
					}

					// split
					if (subRange.RangeStart > subtracted[i].RangeStart && subRange.RangeEnd < subtracted[i].RangeEnd)
					{
						// completly inside of range
						var left = new RevRange { RangeStart = subtracted[i].RangeStart, RangeEnd = subRange.RangeStart - 1 };
						var right = new RevRange { RangeStart = subRange.RangeEnd + 1, RangeEnd = subtracted[i].RangeEnd };

						subtracted[i] = left;
						subtracted.Insert(i + 1, right);

						// can't cut other ranges
						break;
					}

					// left cut
					if (subRange.RangeEnd >= subtracted[i].RangeStart && subRange.RangeEnd < subtracted[i].RangeEnd)
					{
						subtracted[i] = new RevRange { RangeStart = subRange.RangeEnd + 1, RangeEnd = subtracted[i].RangeEnd };
						// can't cut other ranges
						break;
					}

					// right cut
					if (subRange.RangeStart > subtracted[i].RangeStart && subRange.RangeStart <= subtracted[i].RangeEnd)
					{
						subtracted[i] = new RevRange { RangeStart = subtracted[i].RangeStart, RangeEnd = subRange.RangeStart - 1 };
						// can cut next range
					}
				}
			}

			return new RevRangeSet(subtracted).Normalize();
		}

		public bool ContainsRevision(long revision)
		{
			return _ranges.Any(range => revision >= range.RangeStart && revision <= range.RangeEnd);
		}
	}
}
