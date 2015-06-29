using System;
using TrackGearLibrary.Rally;

namespace TrackGearLibrary
{
	public class TicketItem
	{
		private readonly int _ticketNumber;
		
		public string Status;
		public string Assignee;
		public int PriorityIndex;
		public string Severity;
		public int SeverityIndex;
		public string Resolution;
		public string Deadline;
		public string Reporter;
		public string ResolvedBy;
		public DateTime? ResolvedDate;
		public DateTime? LastModified;
		public DateTime Created;
		public int[] Originators;
		public string Token;

		public bool RallyItem;
		public string RallyFormattedID;
		public string RallyUrl;
		public Artifact RallyArtifact;

		public TicketItem(int ticketNumber, string project, string ticketSummary)
		{
			_ticketNumber = ticketNumber;
			Summary = ticketSummary;
			Project = project;
		}

		public int Number
		{
			get { return _ticketNumber; }
		}

		public string Project { get; set; }

		public int ProjectId { get; set; }

		public string Summary;
	}
}
