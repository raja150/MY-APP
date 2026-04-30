using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Payroll
{
	public partial interface IIncentivesPayCutService : IBaseService<IncentivesPayCut>
	{
		Task<Result<Dictionary<string, int>>> AddBulk(List<IncentivesPayCut> items);
		Task<IncentivesPayCut> GetByEmployee(Guid empId, int month, int year);

	}
	public partial class IncentivesPayCutService : BaseService<IncentivesPayCut>, IIncentivesPayCutService
	{

		public IncentivesPayCutService(IUnitOfWork uow) : base(uow)
		{

		}

		public override async Task<IncentivesPayCut> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<IncentivesPayCut>().SingleAsync(x => x.ID == id,
				include: x => x.Include(x => x.Employee));
		}

		public override async Task<IPaginate<IncentivesPayCut>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<IncentivesPayCut>().GetPageListAsync(
				 predicate: x => string.IsNullOrEmpty(baseSearch.Name) || x.Employee.Name.Contains(baseSearch.Name) 
				|| x.Employee.No.Contains(baseSearch.Name),
				   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
				.Include(x => x.Employee).ThenInclude(x => x.Designation),
				 index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Employee.No", ascending: !baseSearch.IsDescend);
		}
		public override async Task<Result<IncentivesPayCut>> UpdateAsync(IncentivesPayCut item)
		{
			IncentivesPayCut entity = await UOW.GetRepositoryAsync<IncentivesPayCut>().SingleAsync(x => x.ID == item.ID);

			Result<IncentivesPayCut> result = new();
			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid));
				return result;
			}
			else
			{
				entity.Update(item);
			}

			return await base.UpdateAsync(entity);
		}
		public override Task OnBeforeAdd(IncentivesPayCut item, Result<IncentivesPayCut> executionResult)
		{
			item.Incentives = item.SumOfIncentives();
			item.PayCut = item.SumOfPayCut();
			return base.OnBeforeAdd(item, executionResult);
		}
		public async Task<Result<Dictionary<string, int>>> AddBulk(List<IncentivesPayCut> items)
		{
			var result = new Result<Dictionary<string, int>>();
			var entityList = await UOW.GetRepositoryAsync<IncentivesPayCut>().GetAsync(x => x.Month == items.FirstOrDefault().Month
			&& x.Year == items.FirstOrDefault().Year);

			//Add or Update Components  
			CollectionCompareResult<IncentivesPayCut> compareList = entityList.Compare(items, (x, y) => x.EmployeeId.Equals(y.EmployeeId));
			foreach (IncentivesPayCut entity in compareList.Same)
			{
				IncentivesPayCut editItem = items.FirstOrDefault(x => x.EmployeeId == entity.EmployeeId);
				if (editItem != null && !entity.Equals(editItem))
				{
					entity.Update(editItem);
					UOW.GetRepositoryAsync<IncentivesPayCut>().UpdateAsync(entity);
				}
			}
			foreach (IncentivesPayCut comp in compareList.Added)
			{
				comp.Incentives = comp.SumOfIncentives();
				comp.PayCut = comp.SumOfPayCut();
				if (comp.Incentives > 0 || comp.PayCut > 0)
					await UOW.GetRepositoryAsync<IncentivesPayCut>().AddAsync(comp);
			}

			foreach (IncentivesPayCut comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<IncentivesPayCut>().DeleteAsync(comp);
			}
			try
			{
				await UOW.SaveChangesAsync();

				var query = UOW.GetRepositoryAsync<IncentivesPayCut>().Queryable(predicate: x => x.Month == items.FirstOrDefault().Month
																					&& x.Year == items.FirstOrDefault().Year).ToList();
				var values = query.GroupBy(g => new { g.Month }).Select(s => new Dictionary<string, int>
				{
					{"Employees",s.Count() },
					{"Incentives", s.Sum(x => x.Incentives)},
					{"PayCut", s.Sum(x => x.PayCut) }
				}).ToList().FirstOrDefault();

				result.ReturnValue = values;
				return result;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}
		public async Task<IncentivesPayCut> GetByEmployee(Guid empId, int month, int year)
		{
			return await UOW.GetRepositoryAsync<IncentivesPayCut>().SingleAsync(x => x.EmployeeId == empId && x.Month == month && x.Year == year);
		}
		public override async Task CustomValidation(IncentivesPayCut item, Result<IncentivesPayCut> result)
		{
			if (await UOW.GetRepositoryAsync<IncentivesPayCut>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId && x.Month == item.Month && x.Year == item.Year && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Incentives_And_PayCut_Already_Exist));
				return;
			}
		}

	}
}
