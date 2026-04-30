using System;

namespace TranSmart.Domain.Models.Helpdesk.Request	
{
	public class TicketLogRecipientsModel : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string WorkMail { get; set; }
	}
}
