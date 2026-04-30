using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.SelfService;

namespace TranSmart.Service.SelfService
{
	public interface ICompilanceService : IBaseService<Compliance>
	{

		Task<Result<Compliance>> AddOrUpdate(Compliance item);
	}

	public class CompilanceService : BaseService<Compliance>, ICompilanceService
	{
		public CompilanceService(IUnitOfWork uow) : base(uow)
		{

		}
		public override async Task<Compliance> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<Compliance>().SingleAsync(x => x.EmployeeId == id);
		}
		public async Task<Result<Compliance>> AddOrUpdate(Compliance item)
		{
			var document = await UOW.GetRepositoryAsync<Compliance>().SingleAsync(x => x.EmployeeId == item.EmployeeId);
			if (document == null)
			{
				return await base.AddAsync(item);

			}
			else
			{
				document.FileName = item.FileName;
				return await base.UpdateAsync(document);
			}
		}
	}
}
