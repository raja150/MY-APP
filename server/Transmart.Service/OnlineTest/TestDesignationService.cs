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
	public interface ITestDesignationService : IBaseService<TestDesignation>
	{
		Task<Result<TestDesignation>> DeleteDesignation(Guid Id);
	}
	public class TestDesignationService : BaseService<TestDesignation>, ITestDesignationService
	{
		public TestDesignationService(IUnitOfWork uow) : base(uow)
		{

		}

		public override async Task<IPaginate<TestDesignation>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<TestDesignation>().GetPageListAsync(predicate: x => x.PaperId == baseSearch.RefId && !x.IsDelete,
				include: i => i.Include(x => x.Designation),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Designation.Name", ascending: !baseSearch.IsDescend);
		}

		public override async Task<Result<TestDesignation>> AddAsync(TestDesignation item)
		{
			var result = new Result<TestDesignation>();
			var entity = await UOW.GetRepositoryAsync<TestDesignation>().SingleAsync(x => x.ID != item.ID &&
			x.DesignationId == item.DesignationId && x.PaperId == item.PaperId && !x.IsDelete);

			if (entity != null)
			{
				result.AddMessageItem(new MessageItem(nameof(item.DesignationId), Resource.Designation_Already_Exists_In_The_Paper));
				return result;
			}
			return await base.AddAsync(item);

		}

		public async Task<Result<TestDesignation>> DeleteDesignation(Guid Id)
		{
			var result = new Result<TestDesignation>();
			try
			{
				var desig = await UOW.GetRepositoryAsync<TestDesignation>().SingleAsync(x => x.ID == Id);
				if (desig == null)
				{
					result.AddMessageItem(new MessageItem(Resource.Designation_Not_Exist_in_the_Paper));
					return result;
				}
				desig.IsDelete = true;
				UOW.GetRepositoryAsync<TestDesignation>().UpdateAsync(desig);
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
