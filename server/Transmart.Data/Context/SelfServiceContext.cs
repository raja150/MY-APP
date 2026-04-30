using Microsoft.EntityFrameworkCore;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.SelfService;

namespace TranSmart.Data
{
	public partial class TranSmartContext
	{
		public DbSet<ApplyClientVisits> SelfService_ApplyClientVisits { get; set; }
		public DbSet<ApplyLeave> SelfService_ApplyLeaves { get; set; }
		public DbSet<Ticket> SelfService_Ticket { get; set; }
		public DbSet<TicketLog> HelpDesk_TicketLog { get; set; }
		public DbSet<TicketLogRecipients> HelpDesk_TicketLogRecipients { get; set; }
		public DbSet<EmpImage> EmpImages { get; set; }
		public DbSet<Compliance> SS_Compliance { get; set; }	
	}

}
