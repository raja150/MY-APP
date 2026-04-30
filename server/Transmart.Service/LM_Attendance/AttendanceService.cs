using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository.Attendance;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.LM_Attendance;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Domain.Models.LM_Attendance.Responce;
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;

namespace TranSmart.Service.Leave
{
	public interface IAttendanceService : IBaseService<Attendance>
	{
		Task<Attendance> GetAttendanceReport(Guid EmpId, DateTime AttDate);
		Task<IEnumerable<Attendance>> GetAttendance(DateTime FromDate, DateTime ToDate);
		Task<Result<Attendance>> AttendanceFromImport(List<Attendance> items);
		Task<IPaginate<Employee>> GetAttendanceData(BaseSearch search);
		Task<Result<ManualAttLogs>> ManualLogsImport(List<ManualAttLogs> items);
		Task<Result<BiometricAttLogs>> BiometricLogsImport(List<BiometricAttLogs> items);
		Task<Result<Attendance>> CalculateAttendance(DateTime AttDate);
		Task<Attendance> GetDate(Guid id);
		Task<Attendance> GetPunchIn(Guid EmpId);
		Task<Result<Attendance>> AddNewTimings(Attendance items);
		Task<Result<Attendance>> UpdateTimings(Guid employeeId);
		Task<Result<Attendance>> RePunchIn(Guid empId);
		Task<Organizations> GetOrganizations();
		Task<bool> IsPunchEmployee(Guid empId);
		Task<Result<Attendance>> AttendanceUpdate(List<AttendanceDetails> items, Guid approverId);
		Task<Attendance> ChangeAttStatus(Attendance att, int attStatus, decimal presentVal, decimal absentVal, decimal leaveVal, decimal wfhVal, bool isHalfDay, int halfDayType, bool isFirstfOff, Guid? LeaveTypeId, int UnAutherized);
		Task<Result<LeaveBalance>> LeavesCalculation(Attendance presentAttendance, AttendanceDetails requiredAtt, Guid approverId);
		Task<Attendance> GetAttWithSchedule(Attendance att);
		Task<Result<AttendanceSum>> Finalized(byte month, short year);
		Task<List<WeekOffDays>> GetWeekOffDays(DateTime attendanceDate);
		Task<IEnumerable<BiometricAttLogs>> GetBiometricMovement(DateTime attendanceDate, Guid empId);
		Task<IEnumerable<BiometricAttLogs>> GetManualAttendanceMovement(DateTime attendanceDate, Guid empId);
		Task<IEnumerable<EmployeeSummaryModel>> EmployeeSummary(Guid? empId, DateTime fromDate, DateTime toDate);
		Task<IEnumerable<EmployeeSummaryModel>> TeamSummary(Guid? empId, DateTime fromDate, DateTime toDate, Guid loginUserId);
		int GetAttRemainingHalfDay(int PresentStatus);
		Task<Result<Attendance>> AddAttendance(IEnumerable<Attendance> items);
	}
	public class AttendanceService : BaseService<Attendance>, IAttendanceService
	{
		private readonly IScheduleService _scheduleService;
		private readonly ILeaveBalanceService _leaveBalanceAddUpDate;
		private readonly IAttendanceRepository _attendanceRepository;
		public AttendanceService(IUnitOfWork uow,
			IScheduleService scheduleService,
			ILeaveBalanceService leaveBalanceService,
			IAttendanceRepository attendanceRepository) : base(uow)
		{
			_scheduleService = scheduleService;
			_leaveBalanceAddUpDate = leaveBalanceService;
			_attendanceRepository = attendanceRepository;
		}

		public virtual async Task<Attendance> GetAttendanceReport(Guid EmpId, DateTime AttDate)
		{
			return await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == EmpId && x.AttendanceDate.Date == AttDate.Date);
		}
		public async Task<Attendance> GetPunchIn(Guid EmpId)
		{
			Attendance result = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == EmpId && x.AttendanceDate.Date == DateTime.Today,
			include: x => x.Include(x => x.Employee));
			return result;
		}
		public async Task<bool> IsPunchEmployee(Guid empId)
		{
			var employee = await UOW.GetRepositoryAsync<Employee>().SingleAsync(predicate: p => p.ID == empId);
			if (employee == null || !employee.AllowWebPunch)
			{
				return false;
			}
			return true;
		}
		public async Task<IEnumerable<BiometricAttLogs>> GetBiometricMovement(DateTime attendanceDate, Guid empId)
		{
			return await UOW.GetRepositoryAsync<BiometricAttLogs>().GetAsync(x => x.AttendanceDate.Date == attendanceDate.Date && x.EmployeeId == empId);
		}
		public async Task<IEnumerable<BiometricAttLogs>> GetManualAttendanceMovement(DateTime attendanceDate, Guid empId)
		{
			List<BiometricAttLogs> result = new List<BiometricAttLogs>();
			var att = await UOW.GetRepositoryAsync<ManualAttLogs>().SingleAsync(x => x.AttendanceDate.Date == attendanceDate.Date && x.EmployeeId == empId);
			if (att != null)
			{
				result.Add(new BiometricAttLogs { AttendanceDate = attendanceDate, MovementTime = att.InTime, MovementType = (int)AttMovementType.In });
				result.Add(new BiometricAttLogs { AttendanceDate = attendanceDate, MovementTime = att.OutTime, MovementType = (int)AttMovementType.Out });
			}

			return result;
		}
		public async Task<IPaginate<Employee>> GetAttendanceData(BaseSearch search)
		{
			AttendanceSearch attendanceSearch = (AttendanceSearch)search;
			return await UOW.GetRepositoryAsync<Employee>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(attendanceSearch.EmpId.ToString()) || x.ID == attendanceSearch.EmpId)
						&& (string.IsNullOrEmpty(attendanceSearch.Department.ToString()) || x.DepartmentId == attendanceSearch.Department)
						&& (string.IsNullOrEmpty(attendanceSearch.Designation.ToString()) || x.DesignationId == attendanceSearch.Designation)
						&& (x.Status == 1 || (x.LastWorkingDate >= attendanceSearch.FromDate &&
						x.LastWorkingDate <= attendanceSearch.ToDate)) && x.WorkType.CalculateAtt == true,
				include: x => x.Include(i => i.Department).Include(i => i.Designation)
				.Include(i => i.WorkType)
							   .Include(i => i.Attendances.Where(o => o.AttendanceDate.Date >= attendanceSearch.FromDate.Date
											&& o.AttendanceDate.Date <= attendanceSearch.ToDate.Date)).ThenInclude(o => o.LeaveType),
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Name", ascending: !search.IsDescend);
		}
		public async Task<Result<Attendance>> AddNewTimings(Attendance items)
		{
			Result<Attendance> result = await base.AddAsync(items);
			return result;
		}
		public async Task<Result<Attendance>> UpdateTimings(Guid employeeId)
		{
			Result<Attendance> result = new();
			Attendance attendance = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == employeeId && x.AttendanceDate.Date == DateTime.Today);
			if (attendance != null)
			{
				attendance.OutTime = DateTime.Now;
				attendance.WorkTime = DateTime.Now.Subtract((DateTime)attendance.InTime).Hours;
				result = await base.UpdateAsync(attendance);
				return result;
			}
			result.AddMessageItem(new MessageItem("Invalid Data"));
			return result;
		}
		public async Task<Result<Attendance>> RePunchIn(Guid empId)
		{
			Result<Attendance> result = new();
			var attendance = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == empId && x.AttendanceDate.Date == DateTime.Today);
			if (attendance != null)
			{
				attendance.InTime = DateTime.Now;
				attendance.OutTime = null;
				attendance.WorkTime = 0;
				result = await base.UpdateAsync(attendance);
				return result;
			}
			result.AddMessageItem(new MessageItem("Invalid Data"));
			return result;
		}
		public virtual async Task<Result<Attendance>> AttendanceFromImport(List<Attendance> items)
		{
			Result<Attendance> result = new(false);
			foreach (Attendance item in items)
			{
				ApplyLeave empAppliedLeaves = await UOW.GetRepositoryAsync<ApplyLeave>()
						.SingleAsync(a => a.EmployeeId == item.EmployeeId && a.FromDate <= item.AttendanceDate
							&& a.ToDate >= item.AttendanceDate && a.Status == 1);
				if (empAppliedLeaves == null)
				{
					if (item.ID == Guid.Empty)
					{
						item.ID = Guid.NewGuid();
						result = await base.AddAsync(item);
					}
					else
					{
						result = await base.UpdateAsync(item);
					}
				}

				if (!result.IsSuccess)
					return result;
			}
			return result;
		}

		public override async Task<IPaginate<Attendance>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<Attendance>().GetPageListAsync(
				predicate: x => string.IsNullOrEmpty(baseSearch.Name) || x.Employee.Name.Contains(baseSearch.Name),
				sortBy: "AddedAt",
				include: i => i.Include(x => x.Employee),
				index: baseSearch.Page, size: baseSearch.Size);
		}
		public async Task<IEnumerable<Attendance>> GetAttendance(DateTime FromDate, DateTime ToDate)
		{
			return await UOW.GetRepositoryAsync<Attendance>().GetAsync(a => a.AttendanceDate.Date >= FromDate.Date && a.AttendanceDate.Date <= ToDate.Date,
				 include: o => o.Include(x => x.Employee).ThenInclude(d => d.Designation).Include(i => i.LeaveType));

		}


		public async Task<Result<ManualAttLogs>> ManualLogsImport(List<ManualAttLogs> items)
		{
			Result<ManualAttLogs> result = new();
			foreach (var item in items)
			{
				if (await UOW.GetRepositoryAsync<ManualAttLogs>().GetCountAsync(x => x.EmployeeId == item.EmployeeId && x.AttendanceDate == item.AttendanceDate) == 0)
					await UOW.GetRepositoryAsync<ManualAttLogs>().AddAsync(item);
			}
			await UOW.SaveChangesAsync();
			return result;
		}
		public async Task<Result<BiometricAttLogs>> BiometricLogsImport(List<BiometricAttLogs> items)
		{
			Result<BiometricAttLogs> result = new();
			var Employees = (await UOW.GetRepositoryAsync<Employee>().GetAsync(x => x.Status == 1 || x.Status == 2,
								  include: i => i.Include(x => x.WorkLocation)
								  .Include(x => x.Department)
								  .Include(x => x.Designation)
								  .Include(x => x.Team))).ToList();
			foreach (var item in items)
			{
				if (await UOW.GetRepositoryAsync<BiometricAttLogs>().GetCountAsync(x => x.EmpCode == item.EmpCode && x.MovementTime == item.MovementTime) == 0 && Employees.Any(x => GetNumaricEmpCode(x.No) == item.EmpCode))
				{
					var schedule = await _scheduleService.GetEmployeeSchedule(Employees.First(x => GetNumaricEmpCode(x.No) == item.EmpCode));

					if (schedule.StartAt > 0)
					{
						// Adding -5 hours to  InTime
						//Based on ExpInTime defining the AttendaceDate for that Record
						var ExpInTime = schedule.StartAt - 300 < 0 ? (1440 + (schedule.StartAt - 300)) : (schedule.StartAt - 300);
						var DayInTime = item.MovementTime.Date.AddMinutes(ExpInTime);
						item.AttendanceDate = item.MovementTime < DayInTime ? item.MovementTime.AddDays(-1).Date : item.MovementTime.Date;
					}
					else
					{
						item.AttendanceDate = item.MovementTime.Date;
					}
					await UOW.GetRepositoryAsync<BiometricAttLogs>().AddAsync(item);
				}
			}
			await UOW.SaveChangesAsync();

			await _attendanceRepository.UpdateEmpInAttLogs();
			return result;
		}
		public async Task<Attendance> GetDate(Guid id)
		{
			return await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == id);
		}
		public async Task<Organizations> GetOrganizations()
		{
			return await UOW.GetRepositoryAsync<Organizations>().SingleAsync();
		}
		public async Task<Result<AttendanceSum>> Finalized(byte month, short year)
		{
			var result = new Result<AttendanceSum>();
			var paymonth = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.Month == month && x.Year == year
										&& (x.Status == (int)PayMonthStatus.Open || x.Status == (int)PayMonthStatus.InProcess));
			var existAttendanceSum = await UOW.GetRepositoryAsync<AttendanceSum>().GetAsync(x => x.Month == month && x.Year == year);

			if (paymonth == null || paymonth.Status != (int)PayMonthStatus.InProcess)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_PayMonth));
				return result;
			}
			var attendanceSum = _attendanceRepository.Summary(paymonth.Start, paymonth.End, month, year);
			if (existAttendanceSum.Any())
			{
				foreach (var item in existAttendanceSum)
				{
					var response = attendanceSum.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
					item.Update(response);
					UOW.GetRepositoryAsync<AttendanceSum>().UpdateAsync(item);
				}
				await UOW.SaveChangesAsync();
			}
			else
			{
				await UOW.GetRepositoryAsync<AttendanceSum>().AddAsync(attendanceSum);
				await UOW.SaveChangesAsync();
			}
			return result;
		}

		#region Calculating Attendance

		public async Task<Result<Attendance>> CalculateAttendance(DateTime AttDate)
		{
			var result = new Result<Attendance>();

			var calculate = new CalculateAttendance(UOW, _scheduleService)
			{
				// Getting Attendance records
				Attendances = (await GetAttendance(AttDate, AttDate)).ToList(),

				// Getting Biometric Logs
				BiometricLogs = UOW.GetRepositoryAsync<BiometricAttLogs>().GetAsync(x => x.AttendanceDate.Date == AttDate.Date).Result.ToList()
			};

			// Getting Manual Logs from tranSmart (first file open and file close time)
			calculate.ManualAttLogs = UOW.GetRepositoryAsync<ManualAttLogs>().GetAsync(x => x.AttendanceDate.Date == AttDate).Result.ToList();

			// Getting all employees
			calculate.Employees = (await UOW.GetRepositoryAsync<Employee>().GetAsync(x => (x.Status == 1 || (x.Status == 2 && x.LastWorkingDate >= AttDate)) && x.WorkType.CalculateAtt == true,
								  include: i => i.Include(x => x.WorkLocation)
								  .Include(x => x.Department)
								  .Include(x => x.Designation)
								  .Include(x => x.Team)
								  .Include(x => x.WorkType))).ToList();
			calculate.WeekOffDays = await GetWeekOffDays(AttDate.Date);
			calculate.WorkHoursSettings = await UOW.GetRepositoryAsync<WorkHoursSetting>().GetAsync(x => x.Status);
			calculate.Isholiday = await UOW.GetRepositoryAsync<Holidays>().SingleAsync(x => x.Date.Date == AttDate.Date);
			calculate.ClientVisits = (await UOW.GetRepositoryAsync<ApplyClientVisits>().GetAsync(x => x.FromDate.Date <= AttDate && x.ToDate.Date >= AttDate && x.Status == 2)).ToList();

			//calculate.Leaves = await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync
			//	(x => x.FromDate.Date <= AttDate.Date && x.ToDate.Date >= AttDate.Date && x.Status == 2);
			calculate.LeaveTypes = await UOW.GetRepositoryAsync<LeaveType>().GetAsync(predicate: x => x.Status);
			calculate.Leaves = await UOW.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync
				(predicate: x => x.LeaveDate.Date == AttDate.Date && x.ApplyLeave.Status == 2,
							include: i => i.Include(x => x.ApplyLeave));

			calculate.Exemption = calculate.Isholiday == null ? new Exemptions() :
					await UOW.GetRepositoryAsync<Exemptions>().SingleAsync(x => x.HolidaysId == calculate.Isholiday.ID);

			calculate.WFH = (await UOW.GetRepositoryAsync<ApplyWfh>().GetAsync(x => x.FromDateC.Date <= AttDate && x.ToDateC.Date >= AttDate && x.Status == 2)).ToList();

			calculate.Allocations = (await UOW.GetRepositoryAsync<Allocation>().GetAsync(x => x.Employee.Status == 1)).ToList();

			calculate.LeavesSettings = (await UOW.GetRepositoryAsync<LeaveSettings>().GetAsync()).OrderByDescending(x => x.AddedAt).FirstOrDefault();

			try
			{
				var _attendances = await calculate.Process(AttDate.Date);
				await UOW.GetRepositoryAsync<Attendance>().AddAsync(_attendances);
				await UOW.SaveChangesAsync();
			}
			catch (Exception Ex)
			{
				result.AddMessageItem(new MessageItem(Ex));
			}
			return result;
		}

		public async Task<List<WeekOffDays>> GetWeekOffDays(DateTime attendanceDate)
		{
			return (await UOW.GetRepositoryAsync<WeekOffDays>().GetAsync(x => x.WeekOffSetup.Status
					&& ((x.Type == 3 && x.WeekDate == attendanceDate)
					|| x.Type != 3))).ToList();
		}

		#endregion Calculating Attendance

		#region Manual Update
		public async Task<Result<Attendance>> AttendanceUpdate(List<AttendanceDetails> items, Guid approverId)
		{
			Result<Attendance> result = new(false);
			foreach (AttendanceDetails item in items)
			{
				AttendanceStatus modifiedStatus = (AttendanceStatus)Enum.Parse(typeof(AttendanceStatus), item.AttendanceStatus);

				Attendance presentAttendance = await GetAttendanceReport(item.EmployeeId, item.AttendanceDate);
				if (presentAttendance == null)
				{
					continue;
				}
				if (presentAttendance.AttendanceStatus == (int)modifiedStatus)
				{
					result.ReturnValue = presentAttendance;
					result.IsSuccess = true;
				}

				// Leaves adding and updating based on requirement
				Result<LeaveBalance> response = await LeavesCalculation(presentAttendance, item, approverId);
				if (response.HasError)
				{
					result.AddMessageItem(new MessageItem(response.Messages[0].Description));
					return result;
				}

				switch (modifiedStatus)
				{
					case AttendanceStatus.Present:
						//Getting the schedules
						presentAttendance = await GetAttWithSchedule(presentAttendance);
						//updating the attendance status as Present
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.Present, 1, 0, 0, 0, false, 0, false, null, 0);
						break;
					case AttendanceStatus.Absent:
						//updating the attendance status as Absent
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.Absent, 0, 1, 0, 0, false, 0, false, null, 0);
						break;
					case AttendanceStatus.Leave:
						//updating the attendance status as Leave
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.Leave, 0, 0, 1, 0, false, 0, false, item.LeaveTypeId, 0);
						break;
					case AttendanceStatus.WFH:
						//updating the attendance status as WFH 
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.WFH, 0, 0, 0, 1, false, 0, false, null, 0);
						break;
					case AttendanceStatus.WeekOff:
						//updating the attendance status as Week Off
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.WeekOff, 0, 0, 0, 0, false, 0, false, null, 0);
						break;
					case AttendanceStatus.Holiday:
						//updating the attendance status as Holiday
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.Holiday, 0, 0, 0, 0, false, 0, false, null, 0);
						break;

					case AttendanceStatus.HalfDayPresent:
						//Getting the schedules
						presentAttendance = await GetAttWithSchedule(presentAttendance);
						//updating the attendance status as HalfDay Present
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.HalfDayPresent, 0, 0, 0, 0, true, item.HalfDayType, item.IsFirstOff, item.LeaveTypeId, 0);

						break;
					case AttendanceStatus.HalfDayAbsent:
						//updating the attendance status as HalfDay Absent 
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.HalfDayAbsent, 0, 0, 0, 0, true, item.HalfDayType, item.IsFirstOff, item.LeaveTypeId, 0);

						break;
					case AttendanceStatus.HalfDayLeave:
						//updating the attendance status as HalfDay Leave 
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.HalfDayLeave, 0, 0, 0, 0, true, item.HalfDayType, item.IsFirstOff, item.LeaveTypeId, 0);

						break;
					case AttendanceStatus.HalfDayWFH:
						//updating the attendance status as HalDay WFH
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.HalfDayWFH, 0, 0, 0, 0, true, item.HalfDayType, item.IsFirstOff, item.LeaveTypeId, 0);
						break;
					case AttendanceStatus.Unautherized:
						//updating the attendance status as UnAutherized
						await ChangeAttStatus(presentAttendance, (int)AttendanceStatus.Unautherized, 0, 0, 0, 0, item.IsHalfDay, item.HalfDayType, item.IsFirstOff, item.LeaveTypeId, (int)item.Unauthorized);
						break;
					default:
						result.AddMessageItem(new MessageItem("Invalid status"));
						break;
				}
				result.ReturnValue = presentAttendance;
				result.IsSuccess = true;
			}
			try
			{
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex.Message));
			}

			return result;
		}

		public async Task<Result<Attendance>> AddAttendance(IEnumerable<Attendance> items)
		{
			var result = new Result<Attendance>();
			var attendance = await UOW.GetRepositoryAsync<Attendance>().GetAsync();
			foreach (Attendance entity in items)
			{
				await GetAttWithSchedule(entity);
				var existingAtt = attendance.Any(x => x.EmployeeId == entity.EmployeeId && x.AttendanceDate.Date == entity.AttendanceDate.Date);
				if (existingAtt)
				{
					result.AddMessageItem(new MessageItem("Already attendance exist"));
					return result;
				}
			}
			try
			{
				await UOW.GetRepositoryAsync<Attendance>().AddAsync(items);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task<Attendance> ChangeAttStatus(Attendance att, int attStatus, decimal presentVal, decimal absentVal, decimal leaveVal, decimal wfhVal, bool isHalfDay, int halfDayType, bool isFirstfOff, Guid? LeaveTypeId, int UnAutherized)
		{
			AttendanceModifyLogs log = new()
			{
				ID = Guid.NewGuid(),
				PresentAttStatus = (int)att.AttendanceStatus,
				AttendanceID = att.ID,
				IsHalfDay = isHalfDay,
				HalfDayType = halfDayType,
				IsFirstOff = isFirstfOff,
				ModifyStatus = attStatus,
				EmployeeID = att.EmployeeId
			};

			if (att.AttendanceStatus == (int)AttendanceStatus.HalfDayLeave || att.AttendanceStatus == (int)AttendanceStatus.Leave)
			{ log.FromLeaveTypeID = att.LeaveTypeID; }

			if (attStatus == (int)AttendanceStatus.HalfDayLeave
				|| attStatus == (int)AttendanceStatus.Leave
				|| halfDayType == (int)AttendanceStatus.HalfDayLeave
				|| halfDayType == (int)AttendanceStatus.Leave)
			{
				log.ToLeaveTypeID = LeaveTypeId;
				att.LeaveTypeID = LeaveTypeId;
			}
			else if (attStatus == (int)AttendanceStatus.Absent && LeaveTypeId.HasValue)
			{
				att.LeaveTypeID = LeaveTypeId;
			}
			else
			{
				att.LeaveTypeID = null;
			}

			att.AttendanceStatus = attStatus;
			if (!isHalfDay)
			{
				att.IsHalfDay = false;
				att.HalfDayType = halfDayType;
				att.IsFirstHalf = false;
				att.Present = presentVal;
				att.Leave = leaveVal;
				att.Absent = absentVal;
				att.WFH = wfhVal;
				att.UADays = UnAutherized;
			}
			else
			{
				// AttStatus is FirstHalf
				// HalfDayType is SecondHalf
				if (attStatus == (int)AttendanceStatus.HalfDayPresent || halfDayType == (int)AttendanceStatus.HalfDayPresent)
					presentVal = 0.5m;
				if (attStatus == (int)AttendanceStatus.HalfDayWFH || halfDayType == (int)AttendanceStatus.HalfDayWFH)
					wfhVal = 0.5m;
				if (attStatus == (int)AttendanceStatus.HalfDayLeave || halfDayType == (int)AttendanceStatus.HalfDayLeave)
					leaveVal = 0.5m;
				if (attStatus == (int)AttendanceStatus.HalfDayAbsent || halfDayType == (int)AttendanceStatus.HalfDayAbsent)
					absentVal = 0.5m;

				att.IsHalfDay = true;
				att.HalfDayType = halfDayType;
				att.IsFirstHalf = isFirstfOff;
				att.Present = presentVal;
				att.Leave = leaveVal;
				att.Absent = absentVal;
				att.WFH = wfhVal;
				att.UADays = UnAutherized;
			}
			UOW.GetRepositoryAsync<Attendance>().UpdateAsync(att);
			await UOW.GetRepositoryAsync<AttendanceModifyLogs>().AddAsync(log);
			return att;
		}
		public async Task<Result<LeaveBalance>> LeavesCalculation(Attendance presentAttendance, AttendanceDetails requiredAtt, Guid approverId)
		{
			return await _leaveBalanceAddUpDate.LeavesModifyBasedOnAttendance(presentAttendance, requiredAtt, approverId);
		}
		public async Task<Attendance> GetAttWithSchedule(Attendance att)
		{
			Employee emp = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == att.EmployeeId && x.Status == 1,
					include: i => i.Include(x => x.Designation).ThenInclude(x => x.WeekOffSetup)
				   .Include(x => x.Department).ThenInclude(x => x.WeekOffSetup)
				   .Include(x => x.Team).ThenInclude(x => x.WeekOffSetup)
				   .Include(x => x.WorkLocation).ThenInclude(x => x.WeekOffSetup));
			ScheduleDetails schedule = await _scheduleService.GetEmployeeSchedule(emp);
			att.InTime = att.AttendanceDate.Add(TimeSpan.FromMinutes(schedule.StartAt));
			att.OutTime = att.AttendanceDate.Add(TimeSpan.FromMinutes(schedule.EndsAt));
			att.SchBreaks = schedule.NoOfBreaks;
			att.SchBreakTime = schedule.BreakTime;
			att.SchInTimeGrace = schedule.LoginGraceTime;
			att.Breaks = 0;
			att.WorkTime = (int)((DateTime)att.OutTime - (DateTime)att.InTime).TotalMinutes;
			return att;
		}

		#endregion
		public static string GetNumaricEmpCode(string code)
		{
			return code.Replace("AVONTIX", string.Empty, StringComparison.OrdinalIgnoreCase);
		}

		public async Task<IEnumerable<EmployeeSummaryModel>> EmployeeSummary(Guid? empId, DateTime fromDate, DateTime toDate)
		{

			var attendance = await UOW.GetRepositoryAsync<Attendance>().GetAsync(predicate: p => p.EmployeeId == empId
			&& p.AttendanceDate.Date >= fromDate.Date && p.AttendanceDate.Date <= toDate.Date);
			var model = new List<EmployeeSummaryModel>();
			foreach (var item in attendance.GroupBy(x => new { x.EmployeeId, x.AttendanceStatus }).Select(x => x.Key))
			{
				var noOfDays = attendance.Count(x => x.EmployeeId == item.EmployeeId && x.AttendanceStatus == item.AttendanceStatus);
				model.Add(new EmployeeSummaryModel
				{
					NoOfDays = noOfDays,
					Status = item.AttendanceStatus
				});
			}
			return model;
		}
		public async Task<IEnumerable<EmployeeSummaryModel>> TeamSummary(Guid? empId, DateTime fromDate, DateTime toDate, Guid loginUserId)
		{

			var attendance = await UOW.GetRepositoryAsync<Attendance>().GetAsync(predicate: p => (!empId.HasValue || p.EmployeeId == empId)
													   && p.Employee.ReportingToId == loginUserId && p.AttendanceDate.Date >= fromDate.Date
													   && p.AttendanceDate.Date <= toDate.Date);
			var model = new List<EmployeeSummaryModel>();
			foreach (var item in attendance.GroupBy(x => new { x.AttendanceStatus }).Select(x => x.Key))
			{
				var noOfDays = attendance.Count(x => x.AttendanceStatus == item.AttendanceStatus);
				model.Add(new EmployeeSummaryModel
				{
					NoOfDays = noOfDays,
					Status = item.AttendanceStatus
				});
			}
			return model;
		}

		public int GetAttRemainingHalfDay(int PresentStatus)
		{
			if (PresentStatus == (int)AttendanceStatus.Present)
			{
				return (int)AttendanceStatus.HalfDayPresent;
			}
			else if (PresentStatus == (int)AttendanceStatus.Absent)
			{
				return (int)AttendanceStatus.HalfDayAbsent;
			}
			else if (PresentStatus == (int)AttendanceStatus.WFH)
			{
				return (int)AttendanceStatus.HalfDayWFH;
			}
			else if (PresentStatus == (int)AttendanceStatus.Leave)
			{
				return (int)AttendanceStatus.HalfDayLeave;
			}
			else
			{
				return PresentStatus;
			}
		}
	}
}
