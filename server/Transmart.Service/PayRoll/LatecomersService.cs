using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;

namespace TranSmart.Service.PayRoll
{
	public interface ILatecomersService : IBaseService<Latecomers>
	{
		Task<Result<Latecomers>> AddBulk(List<Latecomers> items, int month, int year);
	}
	public class LatecomersService : BaseService<Latecomers>, ILatecomersService
	{
		public LatecomersService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<Result<Latecomers>> AddBulk(List<Latecomers> items, int month, int year)
		{
			var result = new Result<Latecomers>();

			var entityList = await UOW.GetRepositoryAsync<Latecomers>().GetAsync(x => x.Month == month && x.Year == year);

			//Add or Update Components  
			CollectionCompareResult<Latecomers> compareList = entityList.Compare(items, (x, y) => x.EmployeeID.Equals(y.EmployeeID));
			foreach (Latecomers entity in compareList.Same)
			{
				Latecomers editItem = items.FirstOrDefault(x => x.EmployeeID == entity.EmployeeID);
				if (editItem != null && !entity.Equals(editItem))
				{
					entity.Update(editItem);
					UOW.GetRepositoryAsync<Latecomers>().UpdateAsync(entity);
				}
			}
			foreach (Latecomers comp in compareList.Added.Where(x => x.NumberOfDays > 0))
			{
				await UOW.GetRepositoryAsync<Latecomers>().AddAsync(comp);
			}

			foreach (Latecomers comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<Latecomers>().DeleteAsync(comp);
			}

			await UOW.SaveChangesAsync();
			return result;
		}
		public override async Task<Latecomers> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<Latecomers>().SingleAsync(x => x.ID == id, include: i => i.Include(x => x.Employee));
		}
		public override async Task<IPaginate<Latecomers>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<Latecomers>().GetPaginateAsync(
				predicate: x => string.IsNullOrEmpty(baseSearch.Name) 
				|| x.Employee.Name.Contains(baseSearch.Name) || x.Employee.No.Contains(baseSearch.Name),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
				.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt),
				index: baseSearch.Page, size: baseSearch.Size);
		}
		public override async Task<Result<Latecomers>> UpdateAsync(Latecomers item)
		{
			Result<Latecomers> executionResult = new();
			Latecomers latecomers = await UOW.GetRepositoryAsync<Latecomers>().SingleAsync(x => x.ID == item.ID);

			if (latecomers == null)
			{
				executionResult.AddMessageItem(new MessageItem(Resource.Invalid_LateComers));
				return executionResult;
			}
			else
			{
				latecomers.EmployeeID = item.EmployeeID;
				latecomers.NumberOfDays = item.NumberOfDays;
			}

			return await base.UpdateAsync(latecomers);
		}
		public override async Task CustomValidation(Latecomers item, Result<Latecomers> result)
		{
			if (await UOW.GetRepositoryAsync<Latecomers>().HasRecordsAsync(x => x.EmployeeID == item.EmployeeID && x.Month == item.Month && x.Year == item.Year && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Latecomer_Already_Exist));
				return;
			}
		}
	}
}
