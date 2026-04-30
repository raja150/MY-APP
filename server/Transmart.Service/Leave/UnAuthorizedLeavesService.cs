using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.Service.Leave
{
	public partial interface IUnAuthorizedLeavesService : IBaseService<UnAuthorizedLeaves>
	{

	}
	public partial class UnAuthorizedLeavesService : BaseService<UnAuthorizedLeaves>, IUnAuthorizedLeavesService
	{
		public UnAuthorizedLeavesService(IUnitOfWork uow) : base(uow)
		{

		}
		public override async Task CustomValidation( UnAuthorizedLeaves item, Result<UnAuthorizedLeaves> result)
		{
			await base.CustomValidation(item, result);
			
			if (await UOW.GetRepositoryAsync<UnAuthorizedLeaves>().HasRecordsAsync(x => x.ID != item.ID && x.EmployeeId == item.EmployeeId && x.Date == item.Date))
			{
				result.AddMessageItem(new MessageItem(Resource.Employee_UnAuthorized_With_Same_Date_is_already_exists));
			}
		}

		public override async Task<IPaginate<UnAuthorizedLeaves>> GetPaginate(BaseSearch baseSearch)
		{
			UnAuthorizedLeavesSearch unAuthorizedLeavesSearch = (UnAuthorizedLeavesSearch)baseSearch;

			return await UOW.GetRepositoryAsync<UnAuthorizedLeaves>().GetPageListAsync(
				predicate: p => (string.IsNullOrEmpty(baseSearch.Name) || p.Employee.Name.Contains(baseSearch.Name))
						 && (unAuthorizedLeavesSearch.FromDate == null || p.Date.Date >= unAuthorizedLeavesSearch.FromDate.Value.Date)
						 && (unAuthorizedLeavesSearch.ToDate == null || p.Date.Date <= unAuthorizedLeavesSearch.ToDate.Value.Date)
						 && (unAuthorizedLeavesSearch.RefId == null || p.RefId == unAuthorizedLeavesSearch.RefId)
				         && (unAuthorizedLeavesSearch.Status == 0 ? (p.LeaveStatus == 1 || p.LeaveStatus == 2) : p.LeaveStatus == unAuthorizedLeavesSearch.Status),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
								.Include(x => x.Employee).ThenInclude(x => x.Designation),
				sortBy: "AddedAt",
				index: baseSearch.Page, size: baseSearch.Size);
		}
		public override async Task<UnAuthorizedLeaves> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<UnAuthorizedLeaves>().SingleAsync(predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
								.Include(x => x.Employee).ThenInclude(x => x.Designation));
		}
	}
}

