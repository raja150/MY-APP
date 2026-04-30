using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.Leave;

namespace TranSmart.Service.Schedules
{
	public interface IScheduleService : IBaseService<Shift>
	{
		Task<List<ScheduleDetails>> GetSchedules(Guid branchID);
		Task<ScheduleDetails> GetEmployeeSchedule(Employee emp);
	}
	public class ScheduleService : BaseService<Shift>, IScheduleService
	{
		public readonly IShiftService _shiftService;
		public ScheduleService(IUnitOfWork uow, IShiftService service) : base(uow)
		{
			_shiftService = service;
		}
		public virtual async Task<List<ScheduleDetails>> GetSchedules(Guid branchID)
		{
			var list = new List<ScheduleDetails>();
			var employeesList = await UOW.GetRepositoryAsync<Employee>().GetAsync(x => x.Status == 1
				&& x.WorkLocationId == branchID,
				include: i => i.Include(x => x.Team)
				.Include(x => x.Designation)
				.Include(x => x.Department)
				.Include(x => x.WorkLocation));
			var empAllocation = await UOW.GetRepositoryAsync<Allocation>().GetAsync(x => x.Employee.Status == 1);
			var shifts = new Dictionary<Guid, Shift>();


			foreach (Employee emp in employeesList)
			{
				Allocation allocation = empAllocation.SingleOrDefault(x => x.EmployeeId == emp.ID);

				Guid? shiftId;
				if (allocation != null && allocation.ShiftId.HasValue)
				{
					shiftId = allocation.ShiftId;
				}
				else if (emp.Team != null && emp.Team.ShiftId.HasValue)
				{
					shiftId = emp.Team.ShiftId;
				}
				else
				{
					shiftId = emp.Designation.ShiftId ?? emp.Department.ShiftId ??
						emp.WorkLocation.ShiftId;
				}

				if (shiftId.HasValue && !shifts.ContainsKey(shiftId.Value))
				{
					var shiftNew = await UOW.GetRepositoryAsync<Shift>().SingleAsync(s => s.ID == shiftId.Value);
					shifts.Add(shiftId.Value, shiftNew);
				}
				var shift = shifts[shiftId.Value];
				list.Add(new ScheduleDetails
				{
					EmployeeID = emp.ID,
					BreakTime = shift.BreakTime,
					NoOfBreaks = shift.NoOfBreaks,
					StartAt = shift.StartFrom,
					EndsAt = shift.EndsOn,
					NextDayOut = shift.StartFrom > shift.EndsOn ? 1 : 0,
					LoginGraceTime = shift.loginGraceTime
				});
			}
			return list;
		}

		public async Task<ScheduleDetails> GetEmployeeSchedule(Employee emp)
		{
			Shift shift = await _shiftService.GetEmployeShift(emp);
			if (shift != null)
			{
				return new ScheduleDetails
				{
					EmployeeID = emp.ID,
					BreakTime = shift.BreakTime,
					NoOfBreaks = shift.NoOfBreaks,
					StartAt = shift.StartFrom,
					EndsAt = shift.EndsOn,
					NextDayOut = shift.StartFrom > shift.EndsOn ? 1 : 0,
					LoginGraceTime = shift.loginGraceTime, 
				};
			}
			 
			return new ScheduleDetails { EmployeeID = emp.ID };
		}
	}
}
