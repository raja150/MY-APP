using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
	public partial interface IEmpStatutoryService : IBaseService<EmpStatutory>
	{
		Task<Result<EmpStatutory>> AddBulk(List<EmpStatutory> items);
	}

	public partial class EmpStatutoryService : BaseService<EmpStatutory>, IEmpStatutoryService
	{
		public async Task<Result<EmpStatutory>> AddBulk(List<EmpStatutory> items)
		{
			var result = new Result<EmpStatutory>();
			//Add or Update Components
			foreach (var item in items)
			{
				var entity = await UOW.GetRepositoryAsync<EmpStatutory>().SingleAsync(x => x.EmpId == item.EmpId);
				if (entity == null)
				{
					await UOW.GetRepositoryAsync<EmpStatutory>().AddAsync(item);
				}
				else
				{ 
					entity.Update(item);
					UOW.GetRepositoryAsync<EmpStatutory>().UpdateAsync(entity);
				}
			} 
			try
			{
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public override async Task CustomValidation(EmpStatutory item, Result<EmpStatutory> result)
		{
			if (await UOW.GetRepositoryAsync<EmpStatutory>().HasRecordsAsync(x => x.EmpId == item.EmpId && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Employee_Already_Exists));
				return;
			}

			if (await UOW.GetRepositoryAsync<EmpStatutory>().HasRecordsAsync(x => x.EmployeesProvid == item.EmployeesProvid && x.EnablePF == 1 && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Account_Number_Already_Exists));
				return;
			}

			if (await UOW.GetRepositoryAsync<EmpStatutory>().HasRecordsAsync(x => x.UAN == item.UAN && x.EnablePF == 1 && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.UAN_Number_Already_Exists));
				return;
			}
		}

	}
}
