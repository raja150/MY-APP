using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
	public partial interface ITemplateEarningService : IBaseService<TemplateEarning>
	{

	}
	public partial class TemplateEarningService : BaseService<TemplateEarning>, ITemplateEarningService
	{
		public override async Task CustomValidation(TemplateEarning item, Result<TemplateEarning> result)
		{
			if (await UOW.GetRepositoryAsync<TemplateEarning>().HasRecordsAsync(x => x.TemplateId == item.TemplateId && x.ComponentId == item.ComponentId && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(nameof(TemplateEarning.ComponentId), Resource.Component_Already_Exist));
				return;
			}
		}
	}
}
