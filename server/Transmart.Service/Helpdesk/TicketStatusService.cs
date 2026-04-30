using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;

namespace TranSmart.Service.Helpdesk
{
	public partial class TicketStatusService : BaseService<TicketStatus>, ITicketStatusService
	{
		public override async Task CustomValidation(TicketStatus item, Result<TicketStatus> result)
		{
			if (await UOW.GetRepositoryAsync<TicketStatus>().HasRecordsAsync(x => x.IsClosed 
						&& item.IsClosed && x.Status && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Closed_Ticket_Already_Exist));
			}
			await base.CustomValidation(item, result);
		}
	}
}
