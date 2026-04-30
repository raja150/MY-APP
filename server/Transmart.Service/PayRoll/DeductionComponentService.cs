using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
	public partial interface IDeductionComponentService : IBaseService<DeductionComponent>
	{
		Task<IEnumerable<DeductionComponent>> DeductionComponents(string orderBy);
	}

	public partial class DeductionComponentService : BaseService<DeductionComponent>, IDeductionComponentService
	{
		public override async Task OnBeforeAdd(DeductionComponent item, Result<DeductionComponent> executionResult)
		{
			//Components which are created by user is always editable
			item.IsEditable = true;
			await base.OnBeforeAdd(item, executionResult);
		}
		public override async Task OnBeforeUpdate(DeductionComponent item, Result<DeductionComponent> executionResult)
		{
			//Few components should be editable. which are created from earrnings components.
			//Example is food coupons.
			DeductionComponent deductionComponent = await UOW.GetRepositoryAsync<DeductionComponent>().SingleAsync(x => x.ID == item.ID);
			if (deductionComponent != null && deductionComponent.EarningId.HasValue)
			{
				item.EarningId = deductionComponent.EarningId;
				item.IsEditable = deductionComponent.IsEditable;
				item.ProrataBasis = deductionComponent.ProrataBasis;
				item.DeductionPlan = deductionComponent.DeductionPlan;
				item.Deduct = deductionComponent.Deduct;
			}
			await base.OnBeforeUpdate(item, executionResult);
		}

		public async Task<IEnumerable<DeductionComponent>> DeductionComponents(string orderBy)
		{
			return await UOW.GetRepositoryAsync<DeductionComponent>().GetListAsync(orderBy, x => x.EarningId == null && x.Status == true);
		}

	}
}
