using System;
using System.Text.RegularExpressions;

namespace TrackGearLibrary.Core.SoftwareVersion
{
	public class SoftwareVersionComponent : IComparable<SoftwareVersionComponent>
	{
		public readonly string Component;

		public readonly string ComparableComponent;

		static readonly Regex NumbeRegex = new Regex(@"^[0-9]+");

		public SoftwareVersionComponent(string comp)
		{
			Component = comp;

			var numericPart = 0;
			string nonNumericPart;

			var m = NumbeRegex.Match(comp);
			if (m.Success && Int32.TryParse(m.Value, out numericPart))
			{
				nonNumericPart = comp.Substring(m.Value.Length);
			}
			else
			{
				nonNumericPart = comp;
			}

			ComparableComponent = string.Format("{0:D10}{1}", numericPart, nonNumericPart);
		}

		public int CompareTo(SoftwareVersionComponent other)
		{
			return String.Compare(ComparableComponent, other.ComparableComponent, StringComparison.Ordinal);
		}

		public override string ToString()
		{
			return Component;
		}
	}

}