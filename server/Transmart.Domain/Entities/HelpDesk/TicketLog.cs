using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.SelfService;

namespace TranSmart.Domain.Entities.HelpDesk
{
	[Table("SS_TicketLog")]
	public class TicketLog : DataGroupEntity
	{
		public Guid TicketId { get; set; }
		public Ticket Ticket { get; set; }
		public Guid? AssignedToId { get; set; }
		public Organization.Employee AssignedTo { get; set; }
		public Guid RepliedById { get; set; }
		public Organization.Employee RepliedBy { get; set; }
		public DateTime RepliedOn { get; set; }
		public string Response { get; set; }

		//1 Post Reply, 2 Dept Transfer, 3 Re-Assign
		public byte TypeOfLog { get; set; }
		public Guid? TicketStatusId { get; set; }
		public Helpdesk.TicketStatus TicketStatus { get; set; }
		public ICollection<TicketLogRecipients> Recipients { get; set; }
	}

}
