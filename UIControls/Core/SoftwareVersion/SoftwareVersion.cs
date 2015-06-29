using System;
using System.Linq;

namespace TrackGearLibrary.Core.SoftwareVersion
{
	public class SoftwareVersion : IComparable<SoftwareVersion>
	{
		public readonly string FullVersion;
		public readonly string ComparableVersion;
		public readonly SoftwareVersionComponent[] VersionComponents;

		public SoftwareVersion(string version)
		{
			FullVersion = version;
			VersionComponents = version.Split('.').Select(v => new SoftwareVersionComponent(v)).ToArray();
			ComparableVersion = string.Join(".", VersionComponents.Select(v => v.ComparableComponent));
		}

		public override string ToString()
		{
			return FullVersion;
		}

		public int CompareTo(SoftwareVersion other)
		{
			return String.Compare(ComparableVersion, other.ComparableVersion, StringComparison.Ordinal);
		}
	}
}