using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Enums
{
	public enum WfhStatus
	{
		Cancelled = 0,
		Applied,
		Approved,
		Rejected

	}
	public enum TicketSts
	{
		Open = 0,
		MyTickets,
		OverDue,
		Closed,
		InProcess
	}
	public enum TicketLogSts
	{
		PostReply = 1,
		Transfer,
		ReAssign,
		UserReply
	}
	public enum Unauthorized
	{
		Unauthorized = 1,
		Authorized
	}
}
