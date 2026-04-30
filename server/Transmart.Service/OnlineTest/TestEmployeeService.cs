using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository.OnlineTest;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models;

namespace TranSmart.Service.OnlineTest
{
	public interface ITestEmployeeService : IBaseService<TestEmployee>
	{
		Task<Result<TestEmployee>> DeleteEmployee(Guid Id);
		Task<IEnumerable<TestEmployee>> GetPaperByEmpId(Guid empId);
	}
	public class TestEmployeeService : BaseService<TestEmployee>, ITestEmployeeService
	{
		public TestEmployeeService(IUnitOfWork uow) : base(uow)
		{
			
		}

		//For display the list of tests through empId
		public async Task<IEnumerable<TestEmployee>> GetPaperByEmpId(Guid empId)
		{
			return await UOW.GetRepositoryAsync<TestEmployee>().GetAsync(x => x.EmployeeId == empId && x.Paper.MoveToLive,
				include: x => x.Include(x => x.Paper));

		}

		public override async Task<IPaginate<TestEmployee>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<TestEmployee>().GetPageListAsync(predicate: x => x.PaperId == baseSearch.RefId && !x.IsDelete,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
				.Include(x => x.Employee).ThenInclude(x => x.Designation),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Employee.Name", ascending: !baseSearch.IsDescend);
		}

		public override async Task<Result<TestEmployee>> AddAsync(TestEmployee item)
		{
			var result = new Result<TestEmployee>();
			var entity = await UOW.GetRepositoryAsync<TestEmployee>().SingleAsync(x => x.ID != item.ID &&
			x.EmployeeId == item.EmployeeId && x.PaperId == item.PaperId && !x.IsDelete);
			if (entity != null)
			{
				result.AddMessageItem(new MessageItem(Resource.Employee_Already_Exists));
				return result;

			}
			return await base.AddAsync(item);
		}

		public async Task<Result<TestEmployee>> DeleteEmployee(Guid testEmpId)
		{
			var result = new Result<TestEmployee>();
			try
			{
				var testEmp = await UOW.GetRepositoryAsync<TestEmployee>().SingleAsync(x => x.ID == testEmpId);
				if (testEmp == null)
				{
					result.AddMessageItem(new MessageItem(Resource.Invalid_Employee));
					return result;
				}
				testEmp.IsDelete = true;
				UOW.GetRepositoryAsync<TestEmployee>().UpdateAsync(testEmp);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}

			return result;
		}
	}
}
