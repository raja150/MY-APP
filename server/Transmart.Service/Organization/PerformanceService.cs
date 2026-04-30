using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
	public interface IPerformanceService : IBaseService<Performance>
	{

	}
	public class PerformanceService : BaseService<Performance>, IPerformanceService
	{
		public PerformanceService(IUnitOfWork uow) : base(uow)
		{
		}
		public override async Task<IPaginate<Performance>> GetPaginate(BaseSearch baseSearch)
		{
			PerformanceSearch search = (PerformanceSearch)baseSearch;
			return await UOW.GetRepositoryAsync<Performance>().GetPageListAsync(
				 predicate: x => (string.IsNullOrEmpty(search.Name) || x.Employee.Name.Contains(search.Name))
								   && (!search.FromDate.HasValue || x.PerformedDate.Date >= search.FromDate.Value)
								   && (!search.ToDate.HasValue || x.PerformedDate.Date <= search.ToDate.Value)
								   && (search.PerformanceType != 0 ? x.PerformanceType == search.PerformanceType : true),
				include: i => i.Include(x => x.Employee),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Employee.No", ascending: !search.IsDescend);
		}
		public override async Task<Performance> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<Performance>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee));
		}
		public override async Task<Result<Performance>> AddAsync(Performance item)
		{
			item.Month = item.PerformedDate.Month;
			item.Year = item.PerformedDate.Year;
			return await base.AddAsync(item);
		}
		public override async Task<Result<Performance>> UpdateAsync(Performance item)
		{
			var result = new Result<Performance>();
			var entity = await UOW.GetRepositoryAsync<Performance>().SingleAsync(x => x.ID == item.ID);
			if (entity == null)
			{
				result.AddMessageItem(new MessageItem("Invalid Item"));
				return result;
			}
			entity.Update(item);
			result = await base.UpdateAsync(entity);
			return result;
		}
	}
}
