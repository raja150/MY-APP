using TranSmart.Data;
using TranSmart.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using TranSmart.Core.Result;
using System.Threading.Tasks;

namespace TranSmart.Service
{
	public interface ILookupService : IBaseService<LookUpValues>
	{

	}

	public class LookupService : BaseService<LookUpValues>, ILookupService
	{
		public LookupService(IUnitOfWork uow) : base(uow)
		{

		}
		public override async Task OnBeforeAdd(LookUpValues item, Result<LookUpValues> executionResult)
		{
			var count = await UOW.GetRepositoryAsync<LookUpValues>().GetCountAsync(x => x.Code == item.Code);
			item.Value = count + 1;
			await base.OnBeforeAdd(item, executionResult);
		}

		public override Task<IEnumerable<LookUpValues>> Search(string name)
		{
			return UOW.GetRepositoryAsync<LookUpValues>().GetAsync(x => x.Code == name,
				orderBy: x => x.OrderBy(o => o.Text));
		}
	}

}
