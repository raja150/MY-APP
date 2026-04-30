using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
	public partial interface IEmpBonusService : IBaseService<EmpBonus>
	{
		Task<Result<Dictionary<string, int>>> AddBulk(List<EmpBonus> items);
	}
	public partial class EmpBonusService : BaseService<EmpBonus>, IEmpBonusService
	{
		public async Task<Result<Dictionary<string, int>>> AddBulk(List<EmpBonus> items)
		{

			//Conditions will update 
			var entityList = await UOW.GetRepositoryAsync<EmpBonus>().GetAsync(x => x.ReleasedOn.Month == items.FirstOrDefault().ReleasedOn.Month
													&& x.ReleasedOn.Year == items.FirstOrDefault().ReleasedOn.Year);

			//Add or Update Components  
			CollectionCompareResult<EmpBonus> compareList = entityList.Compare(items, (x, y) => x.EmployeeId.Equals(y.EmployeeId));
			foreach (EmpBonus entity in compareList.Same)
			{
				EmpBonus editItem = items.FirstOrDefault(x => x.EmployeeId == entity.EmployeeId);
				if (editItem != null && !entity.Equals(editItem))
				{
					entity.Update(editItem);
					UOW.GetRepositoryAsync<EmpBonus>().UpdateAsync(entity);
				}
			}
			foreach (EmpBonus comp in compareList.Added.Where(x => x.Amount > 0))
			{
				await UOW.GetRepositoryAsync<EmpBonus>().AddAsync(comp);
			}

			foreach (EmpBonus comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<EmpBonus>().DeleteAsync(comp);
			}

			await UOW.SaveChangesAsync();
			var result = new Result<Dictionary<string, int>>();
			var query = UOW.GetRepositoryAsync<EmpBonus>().Queryable(predicate: x => x.ReleasedOn.Month == items.FirstOrDefault().ReleasedOn.Month
													&& x.ReleasedOn.Year == items.FirstOrDefault().ReleasedOn.Year);
			var values = (await query.GroupBy(g => new { g.ReleasedOn.Month }).Select(s => new Dictionary<string, int>
			{
				{"Employees" , s.Count()},
				{"Bonus", s.Sum(x => x.Amount)},

			}).ToListAsync()).FirstOrDefault();

			result.ReturnValue = values;
			return result;
		}

		public override async Task CustomValidation(EmpBonus item, Result<EmpBonus> result)
		{
			await base.CustomValidation(item, result);
			var employee = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => item.ReleasedOn >= x.DateOfJoining);
			if (employee == null)
			{
				result.AddMessageItem(new MessageItem(nameof(EmpBonus.ReleasedOn),
					Resource.Release_date_must_be_greater_than_or_equal_to_the_employee_joining_date));
				return;
			}
			if (await UOW.GetRepositoryAsync<EmpBonus>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId && x.ReleasedOn == item.ReleasedOn && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Employee_Bonus_Already_Exist));
				return;
			}
		}
	}
}
