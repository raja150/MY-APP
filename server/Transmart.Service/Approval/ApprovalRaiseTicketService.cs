using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Approval
{
	public partial interface IApprovalTicketService : IBaseService<Ticket>
	{
	}
	public partial class ApprovalTicketService : BaseService<Ticket>, IApprovalTicketService
	{
		public ApprovalTicketService(IUnitOfWork uow) : base(uow)
		{

		}
		public override async Task<IPaginate<Ticket>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<Ticket>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(baseSearch.Name) || x.No.Contains(baseSearch.Name)),
				 include: i => i.Include(x => x.RaiseBy),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "No", ascending: !baseSearch.IsDescend);
		}
	}
}
