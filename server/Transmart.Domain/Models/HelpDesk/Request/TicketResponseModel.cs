using System;

namespace TranSmart.Domain.Models.Helpdesk.Request	
{
	public class TicketResponseModel : BaseModel
	{
		public string No { get; set; }
		public string Response { get; set; }
		public string Raisedby { get; set; }
		public DateTime RaisedOn { get; set; }
		public string Status { get; set; }
		public DateTime RepliedOn { get; set; }
		public string RepliedBy { get; set; }
		public Guid TicketId { get; set; }	
	}
	public class LogResponseModel : BaseModel	
	{
		public string Response { get; set; }
	}
}
