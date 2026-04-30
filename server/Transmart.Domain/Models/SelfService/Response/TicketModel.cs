using System;

namespace TranSmart.Domain.Models.SelfService.Response
{
	public class TicketModel : BaseModel
	{
		public string No { get; set; }
		public Guid RaiseById { get; set; }
		public DateTime RaisedOn { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid HelpTopicId { get; set; }
		public Guid SubTopicId { get; set; }
		public string SubTopic { get; set; }
		public Guid? AssignedToId { get; set; }
		public bool Status { get; set; }
		public int Gender { get; set; }
		public string User { get; set; }
		public int Priority { get; set; }
		public string Email { get; set; }
		public string Department { get; set; }
		public string Phone { get; set; }
		public string Source { get; set; }
		public string HelpTopic { get; set; }
		public string AssignedTo { get; set; }
		public int SLAPlan { get; set; }
		public string LastMessage { get; set; }
		public DateTime DueDate { get; set; }
		public string LastResponse { get; set; }
		public string File { get; set; }
		public string TicketSts { get; set; }
	}
}
