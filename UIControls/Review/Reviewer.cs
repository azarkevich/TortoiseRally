using System.ComponentModel;

namespace RallyToolsCore.Review
{
	public class Reviewer
	{
		[DisplayName("Name")]
		public string Name { get; set; }

		[DisplayName("E-Mail")]
		public string EMail { get; set; }

		public bool Deleted { get; set; }
	}
}
