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
	public interface IArrearService : IBaseService<Arrear>
	{
		Task<Result<Dictionary<string, int>>> AddBulk(List<Arrear> items, int month, int year);

	}
	public class ArrearService : BaseService<Arrear>, IArrearService
	{
		public ArrearService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<Result<Dictionary<string, int>>> AddBulk(List<Arrear> items, int month, int year)
		{

			//Conditions will Update
			var entityList = await UOW.GetRepositoryAsync<Arrear>().GetAsync(x => x.Month == month && x.Year == year);

			//Add or Update Components  
			CollectionCompareResult<Arrear> compareList = entityList.Compare(items, (x, y) => x.EmployeeID.Equals(y.EmployeeID));
			foreach (Arrear entity in compareList.Same)
			{
				Arrear editItem = items.FirstOrDefault(x => x.EmployeeID == entity.EmployeeID);
				if (editItem != null && !entity.Equals(editItem))
				{
					entity.Update(editItem);
					UOW.GetRepositoryAsync<Arrear>().UpdateAsync(entity);
				}
			}
			foreach (Arrear comp in compareList.Added.Where(x => x.Pay > 0))
			{
				await UOW.GetRepositoryAsync<Arrear>().AddAsync(comp);
			}

			foreach (Arrear comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<Arrear>().DeleteAsync(comp);
			}


			await UOW.SaveChangesAsync();
			var resultNew = new Result<Dictionary<string, int>>();
			var query = UOW.GetRepositoryAsync<Arrear>().Queryable(predicate: x => x.Month == month && x.Year == year);
			var values = (await query.GroupBy(g => new { g.Month }).Select(s => new Dictionary<string, int>
			{
				{ "Employees", s.Count() },
				{ "Pay", s.Sum(x => x.Pay)   }
			}).ToListAsync()).FirstOrDefault();

			resultNew.ReturnValue = values;

			return resultNew;
		}
		public override async Task<Arrear> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<Arrear>().SingleAsync(x => x.ID == id, include: i => i.Include(x => x.Employee));
		}
		public override async Task<IPaginate<Arrear>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<Arrear>().GetPaginateAsync(
				predicate: x => string.IsNullOrEmpty(baseSearch.Name)
				||  x.Employee.Name.Contains(baseSearch.Name) || x.Employee.No.Contains(baseSearch.Name),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
				.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt),
				index: baseSearch.Page, size: baseSearch.Size);
		}
		public override async Task<Result<Arrear>> UpdateAsync(Arrear item)
		{
			Result<Arrear> executionResult = new();
			Arrear arrear = await UOW.GetRepositoryAsync<Arrear>().SingleAsync(x => x.ID == item.ID);

			if (arrear == null)
			{
				executionResult.AddMessageItem(new MessageItem(Resource.Invalid_Arrear));
				return executionResult;
			}
			else
			{
				arrear.EmployeeID = item.EmployeeID;
				arrear.Pay = item.Pay;
			}

			return await base.UpdateAsync(arrear);
		}
		public override async Task CustomValidation(Arrear item, Result<Arrear> result)
		{
			if (await UOW.GetRepositoryAsync<Arrear>().HasRecordsAsync(x => x.EmployeeID == item.EmployeeID && x.Month == item.Month && x.Year == item.Year && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Arrear_Already_Exist));
				return;
			}
		}
	}
}
