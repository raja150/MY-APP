using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models;

namespace TranSmart.Service.OnlineTest
{
	public interface ITestDepartmentService : IBaseService<TestDepartment>
	{
		Task<Result<TestDepartment>> DeleteDept(Guid Id);
	}
	public class TestDepartmentService : BaseService<TestDepartment>, ITestDepartmentService
	{
		public TestDepartmentService(IUnitOfWork uow) : base(uow)
		{

		}


		public override async Task<IPaginate<TestDepartment>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<TestDepartment>().GetPageListAsync(predicate: x => x.PaperId == baseSearch.RefId && !x.IsDelete,
				include: i => i.Include(x => x.Department),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Department.Name", ascending: !baseSearch.IsDescend);
		}


		public override async Task<Result<TestDepartment>> AddAsync(TestDepartment item)
		{
			var result = new Result<TestDepartment>();
			var entity = await UOW.GetRepositoryAsync<TestDepartment>().SingleAsync(x => x.ID != item.ID &&
			x.DepartmentId == item.DepartmentId && x.PaperId == item.PaperId && !x.IsDelete);

			if (entity != null)
			{
				result.AddMessageItem(new MessageItem(nameof(item.DepartmentId), Resource.Department_Already_Exists_In_The_Paper));
				return result;
			}
			return await base.AddAsync(item);
		}


		public async Task<Result<TestDepartment>> DeleteDept(Guid Id)
		{
			var result = new Result<TestDepartment>();
			try
			{
				var dept = await UOW.GetRepositoryAsync<TestDepartment>().SingleAsync(x => x.ID == Id);
				if (dept == null)
				{
					result.AddMessageItem(new MessageItem(Resource.Department_Already_Removed_From_The_Paper));
					return result;
				}
				dept.IsDelete = true;
				UOW.GetRepositoryAsync<TestDepartment>().UpdateAsync(dept);
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
