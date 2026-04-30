using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;

namespace TranSmart.Service.PayRoll
{
	public interface IIncomeTaxLimitService : IBaseService<IncomeTaxLimit>
	{
		Task<Result<IncomeTaxLimit>> AddBulk(List<IncomeTaxLimit> items, int month, int year);
	}
	public class IncomeTaxLimitService : BaseService<IncomeTaxLimit>, IIncomeTaxLimitService
	{
		public IncomeTaxLimitService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<Result<IncomeTaxLimit>> AddBulk(List<IncomeTaxLimit> items, int month, int year)
		{
			var result = new Result<IncomeTaxLimit>();
			//Conditions will Update
			var entityList = await UOW.GetRepositoryAsync<IncomeTaxLimit>().GetAsync(x => x.Month == month
																						&& x.Year == year);

			//Add or Update Components  
			CollectionCompareResult<IncomeTaxLimit> compareList = entityList.Compare(items, (x, y) => x.EmployeeId.Equals(y.EmployeeId));
			foreach (IncomeTaxLimit entity in compareList.Same)
			{
				IncomeTaxLimit editItem = items.FirstOrDefault(x => x.EmployeeId == entity.EmployeeId);
				if (editItem != null && !entity.Equals(editItem))
				{
					entity.Update(editItem);
					UOW.GetRepositoryAsync<IncomeTaxLimit>().UpdateAsync(entity);
				}
			}
			foreach (IncomeTaxLimit comp in compareList.Added.Where(x => x.Amount > 0))
			{
				await UOW.GetRepositoryAsync<IncomeTaxLimit>().AddAsync(comp);
			}

			foreach (IncomeTaxLimit comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<IncomeTaxLimit>().DeleteAsync(comp);
			}

			await UOW.SaveChangesAsync();
			return result;
		}

		public override async Task<IncomeTaxLimit> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<IncomeTaxLimit>().SingleAsync(x => x.ID == id,
				include: i => i.Include(x => x.Employee));
		}
		public override async Task<IPaginate<IncomeTaxLimit>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<IncomeTaxLimit>().GetPaginateAsync(
				predicate: x => string.IsNullOrEmpty(baseSearch.Name) 
				|| x.Employee.Name.Contains(baseSearch.Name) || x.Employee.No.Contains(baseSearch.Name),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
				.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt),
				index: baseSearch.Page, size: baseSearch.Size);
		}
		public override async Task<Result<IncomeTaxLimit>> UpdateAsync(IncomeTaxLimit item)
		{
			Result<IncomeTaxLimit> executionResult = new();
			IncomeTaxLimit incomeTaxLimit = await UOW.GetRepositoryAsync<IncomeTaxLimit>().SingleAsync(x => x.ID == item.ID);

			if (incomeTaxLimit == null)
			{
				executionResult.AddMessageItem(new MessageItem(Resource.Invalid));
				return executionResult;
			}
			else
			{
				incomeTaxLimit.EmployeeId = item.EmployeeId;
				incomeTaxLimit.Amount = item.Amount;
			}

			return await base.UpdateAsync(incomeTaxLimit);
		}
		public override async Task CustomValidation(IncomeTaxLimit item, Result<IncomeTaxLimit> result)
		{
			if (await UOW.GetRepositoryAsync<IncomeTaxLimit>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId && x.Month == item.Month && x.Year == item.Year && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Income_Tax_Limit_Is_Already_exist));
				return;
			}
		}
	}
}
