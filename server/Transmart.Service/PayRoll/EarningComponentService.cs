using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;

namespace TranSmart.Service.Payroll
{
	public partial interface IEarningComponentService : IBaseService<EarningComponent>
	{

	}

	public partial class EarningComponentService : BaseService<EarningComponent>, IEarningComponentService
	{
		public override async Task OnBeforeAdd(EarningComponent item, Result<EarningComponent> executionResult)
		{
			//Few component required deduction also like example food coupon , sodexo multi-benefit
			//System is adding default food coupon
			if (item.EarningType == (int)EarningType.FoodCoupon || item.EarningType == (int)EarningType.SodexoMultiBenefit)
			{
				await UOW.GetRepositoryAsync<DeductionComponent>().AddAsync(
					new DeductionComponent
					{
						ID = Guid.NewGuid(),
						Earning = item,
						IsEditable = false,
						Name = item.Name,
						Deduct = 1,
						DeductionPlan = 3,
						ProrataBasis = item.ProrataBasis,
					});
			}

			await base.OnBeforeAdd(item, executionResult);
		}

		public override async Task OnBeforeUpdate(EarningComponent item, Result<EarningComponent> executionResult)
		{
			//While update earning component check refrence deduction component
			//if exists update deduction component IsEditable and ProrataBasis
			DeductionComponent deductionComponent = await UOW.GetRepositoryAsync<DeductionComponent>()
				.SingleAsync(x => x.EarningId == item.ID);
			if (deductionComponent != null)
			{
				deductionComponent.IsEditable = false;
				deductionComponent.ProrataBasis = item.ProrataBasis;
				UOW.GetRepositoryAsync<DeductionComponent>().UpdateAsync(deductionComponent);
			}
			await base.OnBeforeUpdate(item, executionResult);
		}
	}
}
