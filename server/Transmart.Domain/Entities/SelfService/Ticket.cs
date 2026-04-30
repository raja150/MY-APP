using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.SelfService
{
	[Table("SS_Ticket")]
	public class Ticket : DataGroupEntity
	{
		public string No { get; set; }
		public Guid RaiseById { get; set; }
		public Organization.Employee RaiseBy { get; set; }
		public DateTime RaisedOn { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public Guid DepartmentId { get; set; }
		public Helpdesk.DeskDepartment Department { get; set; }
		public Guid HelpTopicId { get; set; }
		public Helpdesk.HelpTopic HelpTopic { get; set; }
		public Guid SubTopicId { get; set; }
		public Helpdesk.HelpTopicSub SubTopic { get; set; }
		public Guid? AssignedToId { get; set; }
		public Organization.Employee AssignedTo { get; set; }
		public string File { get; set; }
		// public byte Status { get; set; }
		public Guid TicketStatusId { get; set; }
		public Helpdesk.TicketStatus TicketStatus { get; set; }

		public void Update(Ticket other)
		{
			Subject = other.Subject;
			Message = other.Message;
			DepartmentId = other.DepartmentId;
			HelpTopicId = other.HelpTopicId;
			SubTopicId = other.SubTopicId;
		}
	}
}
