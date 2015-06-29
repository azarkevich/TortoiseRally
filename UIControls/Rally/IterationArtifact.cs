using System;
using Rally.RestApi;

namespace TrackGearLibrary.Rally
{
	public class IterationArtifact : Artifact
	{
		public static readonly IterationArtifact NullArtifact = new IterationArtifact();

		public DateTimeOffset StartDate = DateTimeOffset.MinValue;
		public DateTimeOffset EndDate = DateTimeOffset.MinValue;

		public bool IsCurrentIteration
		{
			get
			{
				var date = DateTimeOffset.Now.Date;
				return StartDate.Date <= date && EndDate.Date >= date;
			}
		}

		public IterationArtifact(DynamicJsonObject artifact = null)
			: base(artifact)
		{
			if (artifact == null)
				return;

			var sd = TryGetMember<string>(artifact, "StartDate");
			if (sd != null)
				DateTimeOffset.TryParse(sd, out StartDate);

			var ed = TryGetMember<string>(artifact, "EndDate");
			if(ed != null)
				DateTimeOffset.TryParse(ed, out EndDate);
		}
	}
}
