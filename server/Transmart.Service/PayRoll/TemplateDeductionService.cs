using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
	public partial interface ITemplateDeductionService : IBaseService<TemplateDeduction>
	{

	}

	public partial class TemplateDeductionService : BaseService<TemplateDeduction>, ITemplateDeductionService
	{
		public override async Task CustomValidation(TemplateDeduction item, Result<TemplateDeduction> result)
		{
			if (await UOW.GetRepositoryAsync<TemplateDeduction>().HasRecordsAsync(x => x.TemplateId == item.TemplateId && x.ComponentId == item.ComponentId && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(nameof(TemplateDeduction.ComponentId), Resource.Deduction_Component_Already_Exist));
				return;
			}
		}
	}
}
