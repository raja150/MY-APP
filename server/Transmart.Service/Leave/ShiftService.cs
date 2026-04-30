using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Service.Leave
{
	public partial interface IShiftService : IBaseService<Shift>
	{
		Task<Shift> GetEmployeShift(Employee employee);
	}

	public partial class ShiftService : BaseService<Shift>, IShiftService
	{
		public async Task<Shift> GetEmployeShift(Employee employee)
		{
			var allocation = await UOW.GetRepositoryAsync<Allocation>().SingleAsync(x => x.EmployeeId == employee.ID);
			 
			Guid? shiftId;
			if (allocation != null && allocation.ShiftId.HasValue)
			{
				shiftId = allocation.ShiftId;
			}
			else if (employee.Team != null && employee.Team.ShiftId.HasValue)
			{
				shiftId = employee.Team.ShiftId;
			}
			else if (employee.Designation != null && employee.Designation.ShiftId.HasValue)
			{
				shiftId = employee.Designation.ShiftId;
			}
			else
			{
				shiftId = employee.Department != null && employee.Department.ShiftId.HasValue ?
					employee.Department.ShiftId : null;
			}

			if (shiftId.HasValue)
			{
				return await UOW.GetRepositoryAsync<Shift>().SingleAsync(s => s.ID == shiftId);
			}

			return new Shift();
		}
	}
}
