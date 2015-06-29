using System;
using Rally.RestApi;

namespace TrackGearLibrary.Rally
{
	public class Artifact
	{
		static readonly Artifact NullArtifact = new Artifact();

		readonly bool _isNullArtifact;

		public string Reference;
		public ArtifactType Type = ArtifactType.Unknown;
		public long ObjectID;
		public string Version;
		public string FormattedID;
		public string Name;
		public Artifact Parent = NullArtifact;
		public IterationArtifact Iteration = IterationArtifact.NullArtifact;
		public Artifact Project = NullArtifact;
		public Artifact Owner = NullArtifact;

		public static T TryGetNullableMember<T>(DynamicJsonObject rallyArtifact, string name, T def = default(T))
		{
			if (!rallyArtifact.HasMember(name) || rallyArtifact[name] == null)
				return def;

			return rallyArtifact[name];
		}

		public static T TryGetMember<T>(DynamicJsonObject rallyArtifact, string name, T def = default(T))
		{
			if (!rallyArtifact.HasMember(name))
				return def;

			return rallyArtifact[name];
		}

		public bool IsNullArtifact
		{
			get
			{
				return _isNullArtifact;
			}
		}

		public Artifact(DynamicJsonObject rallyArtifact = null)
		{
			if (rallyArtifact == null)
			{
				_isNullArtifact = true;
				return;
			}

			Reference = rallyArtifact["_ref"];

			var type = rallyArtifact["_type"];

			if(type == "Task")
				Type = ArtifactType.Task;
			else if(type == "Defect")
				Type = ArtifactType.Defect;

			Name = rallyArtifact["_refObjectName"];
			Version = rallyArtifact["_objectVersion"];

			FormattedID = TryGetMember<string>(rallyArtifact, "FormattedID");
			ObjectID = TryGetMember<long>(rallyArtifact, "ObjectID");
			if (ObjectID == 0)
			{
				var uri = new Uri(Reference.TrimEnd('/', '\\'));
				ObjectID = long.Parse(uri.Segments[uri.Segments.Length - 1]);
			}

			Project = new Artifact(TryGetNullableMember<dynamic>(rallyArtifact, "Project"));
			Iteration = new IterationArtifact(TryGetNullableMember<dynamic>(rallyArtifact, "Iteration"));
			Owner = new Artifact(TryGetNullableMember<dynamic>(rallyArtifact, "Owner"));

			if (Type == ArtifactType.Defect)
			{
				Parent = new Artifact(TryGetNullableMember<dynamic>(rallyArtifact, "Requirement"));
			}
			else if (Type == ArtifactType.Task)
			{
				Parent = new Artifact(TryGetNullableMember<dynamic>(rallyArtifact, "WorkProduct"));
			}
		}

		public string Url
		{
			get
			{
				if (Type == ArtifactType.Defect)
				{
					return string.Format("https://rally1.rallydev.com/#/{0}/detail/defect/{1}", Project.ObjectID, ObjectID);
				}

				if (Type == ArtifactType.Task)
				{
					return string.Format("https://rally1.rallydev.com/#/{0}/detail/task/{1}", Project.ObjectID, ObjectID);
				}

				return null;
			}
		}
	}
}
