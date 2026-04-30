using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Service.Leave
{
	public partial interface IWorkHoursSettingService : IBaseService<WorkHoursSetting>
	{
		Task<WorkHoursSetting> GetEmployeeWorkHourSetting(Employee employee);
	}
	public partial class WorkHoursSettingService : BaseService<WorkHoursSetting> , IWorkHoursSettingService
	{
		public async Task<WorkHoursSetting>GetEmployeeWorkHourSetting(Employee employee)
		{
			var allocation = await UOW.GetRepositoryAsync<Allocation>().SingleAsync(x => x.EmployeeId == employee.ID);

			Guid? workHourSettingId;
			if (allocation != null && allocation.WorkHoursSettingId.HasValue)
			{
				workHourSettingId = allocation.WorkHoursSettingId;
			}
			else if (employee.Team != null && employee.Team.WorkHoursSettingId.HasValue)
			{
				workHourSettingId = employee.Team.WorkHoursSettingId;
			}
			else if (employee.Designation != null && employee.Designation.WorkHoursSettingId.HasValue)
			{
				workHourSettingId = employee.Designation.WorkHoursSettingId;
			}
			else if(employee.Department !=null && employee.Department.WorkHoursSettingId.HasValue)
			{
				workHourSettingId =employee.Department.WorkHoursSettingId;
			}
			else
			{
				workHourSettingId = employee.WorkLocation != null && employee.WorkLocation.WorkHoursSettingId.HasValue ?
					employee.WorkLocation.WorkHoursSettingId : null;
			}

			if (workHourSettingId.HasValue)
			{
				return await UOW.GetRepositoryAsync<WorkHoursSetting>().SingleAsync(s => s.ID == workHourSettingId);
			}

			return new WorkHoursSetting();
		}
	}
}
