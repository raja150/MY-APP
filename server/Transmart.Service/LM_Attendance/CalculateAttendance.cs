using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.ExtensionMethods;
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.Leave;
using TranSmart.Service.Schedules;

namespace TranSmart.Service.LM_Attendance
{
	public class CalculateAttendance
	{
		private readonly IScheduleService _scheduleService;
		private readonly IUnitOfWork _UOW;
		public CalculateAttendance(
			IUnitOfWork UOW, IScheduleService service)
		{
			_UOW = UOW;
			_scheduleService = service;
		}

		public List<Attendance> Attendances { get; set; }
		public List<BiometricAttLogs> BiometricLogs { get; set; }
		public List<ManualAttLogs> ManualAttLogs { get; set; }
		public List<Employee> Employees { get; set; }
		public Holidays Isholiday { get; set; }
		public Exemptions Exemption { get; set; }
		public List<ApplyWfh> WFH { get; set; }
		public List<Allocation> Allocations { get; set; }
		public List<WeekOffDays> WeekOffDays { get; set; }
		public IEnumerable<ApplyLeaveDetails> Leaves { get; set; }
		public IEnumerable<LeaveType> LeaveTypes { get; set; }
		public LeaveSettings LeavesSettings { get; set; }
		public IEnumerable<WorkHoursSetting> WorkHoursSettings { get; set; }
		public List<ApplyClientVisits> ClientVisits { get; set; }

		public async Task<List<Attendance>> Process(DateTime actionDate)
		{
			var _attendances = new List<Attendance>();

			// Calculating login and logout timings
			foreach (Employee emp in Employees)
			{
				// Here checking  employee having the attendance records or not 
				if (Attendances.Any(a => a.EmployeeId == emp.ID))
				{

					continue;
				}

				// Getting Schedules
				var schedule = await _scheduleService.GetEmployeeSchedule(emp);
				var att = new Attendance()
				{
					ID = Guid.NewGuid(),
					AttendanceDate = actionDate,
					Breaks = 0,
					EmployeeId = emp.ID,
					AddedAt = DateTime.Now
				};

				if (BiometricLogs.Any(x => x.EmployeeId == emp.ID && x.MovementTime.Date == actionDate.Date)
					|| ManualAttLogs.Any(x => x.EmployeeId == emp.ID))
				{
					// update the schedule details if schedule available
					UpdateSchedule(att, schedule);

					// update the IN and OUT time based on logs and schedules
					UpdateInAndOutTime(att, emp.ID);

					// Getting employee applied leaves
					// PayType is 0 then unpaid leave
					// PayType is 1 then paid leave
					var empAppliedLeaves = Leaves.Where(a => a.ApplyLeave.EmployeeId == emp.ID);
					if (!empAppliedLeaves.Any())// having leave on the day
					{
						// Based on Work Hours calculating the FullDay or HalfDay
						AttBasedOnWorkHours(att, emp);
					}
					else
					{
						if (empAppliedLeaves.Sum(x=>x.LeaveCount) == 1m)
						{
							att.AttendanceStatus = (int)AttendanceStatus.Leave;
							att.Leave = 1;
							att.LeaveTypeID = empAppliedLeaves.First().LeaveTypeId;
						}
						else if (empAppliedLeaves.Sum(x => x.LeaveCount) == 0.5m)
						{
							att.IsHalfDay = true;
							att.AttendanceStatus = (int)AttendanceStatus.HalfDayPresent;
							att.Present = 0.5m;
							

							att.IsFirstHalf=empAppliedLeaves.First().IsFirstHalf;
							att.HalfDayType = (int)AttendanceStatus.HalfDayLeave;
							att.Leave = 0.5m;
							att.LeaveTypeID = empAppliedLeaves.First().LeaveTypeId;
						}
						//Mohan Hold updating leaves
						//await LeaveToPresentUpdateLeave(empAppliedLeaves);
					}
				}
				else
				{
					MissedAttendanceCalculation(att, emp, IsEmpHaveExemptionFromHoliday(emp), schedule);
				}

				_attendances.Add(att);
			}

			return _attendances;
		}

		public static Attendance UpdateSchedule(Attendance attendance, ScheduleDetails schedule)
		{
			if (schedule != null && schedule.StartAt != 0)
			{
				attendance.SchBreaks = schedule.NoOfBreaks;
				attendance.SchBreakTime = schedule.BreakTime;
				attendance.SchInTimeGrace = schedule.LoginGraceTime;
				if (schedule.NextDayOut == 0)
				{
					attendance.SchWorkTime = schedule.EndsAt - schedule.StartAt;
				}
				else
				{
					DateTime InTime = attendance.AttendanceDate.Add(TimeSpan.FromMinutes(schedule.StartAt));
					DateTime OutTime = attendance.AttendanceDate.Add(TimeSpan.FromMinutes(schedule.EndsAt)).AddDays(1);
					attendance.SchWorkTime = (int)(OutTime.Subtract(InTime)).TotalMinutes;
				}
				attendance.SchIntime = schedule.StartAt;
				attendance.SchOutTime = schedule.EndsAt;
			}
			return attendance;
		}

		public Attendance UpdateInAndOutTime(Attendance attendance, Guid employeeID)
		{
			List<BiometricAttLogs> bioLogs;
			if (BiometricLogs.Any(x => x.EmployeeId == employeeID))
			{
				bioLogs = BiometricLogs.Where(x => x.EmployeeId == employeeID).ToList();
				//if (attendance.SchIntime.HasValue)
				//{
				//	DateTime suchLoginTime = attendance.AttendanceDate.Date.AddMinutes(schedule.StartAt - 120);
				//	// if scheduled employee is night shift then out time considering as next day
				//	DateTime SuchLogOutTime = schedule.NextDayOut == 1 ?
				//		attendance.AttendanceDate.Date.AddMinutes(schedule.EndsAt + 1560) :
				//		attendance.AttendanceDate.Date.AddMinutes(schedule.EndsAt + 120);
				//	//getting biometric logs based on schedule in and out time
				//	bioLogs = bioLogs.Where(x => x.MovementTime > suchLoginTime && x.MovementTime <= SuchLogOutTime).ToList();
				//}
				//else
				//{
				//	bioLogs = bioLogs.Where(x => x.MovementTime.Date == attendance.AttendanceDate.Date).ToList();
				//}
				if (bioLogs.Count > 0)
				{
					attendance.InTime = bioLogs.Min(x => x.MovementTime);
					attendance.OutTime = bioLogs.Max(x => x.MovementTime);
					attendance = CalculateBreaktime(employeeID, attendance);
					attendance.WorkTime = ((int)((TimeSpan)(attendance.OutTime - attendance.InTime)).TotalMinutes - attendance.BreakTime);
					attendance.LoginType = (byte)LoginType.Biometric;
				}
			}
			else if (ManualAttLogs != null && ManualAttLogs.Any(x => x.EmployeeId == attendance.EmployeeId))
			{
				var logs = ManualAttLogs.First(x => x.EmployeeId == attendance.EmployeeId);
				attendance.InTime = logs.InTime;
				attendance.OutTime = logs.OutTime;
				attendance.WorkTime = (int)((TimeSpan)(attendance.OutTime - attendance.InTime)).TotalMinutes;
				attendance.LoginType = (byte)LoginType.Manual;
			}

			return attendance;
		}
		public Attendance CalculateBreaktime(Guid employeeId, Attendance attendance)
		{
			var bioLogs = BiometricLogs.Where(x => x.EmployeeId == employeeId)
							.Where(x => x.MovementTime > attendance.InTime).ToList();
			double breaktime = 0;
			int NoOffBreaks = 0;
			int PreviousType = 0;
			DateTime PreviousMovementTime = (DateTime)attendance.InTime;
			foreach (var item in bioLogs.OrderBy(x => x.MovementTime))
			{
				// Previous Movement is IN and Present Movement is OUT then not Calculating Break Time
				// Remaining All Cases Break Time is Calculating
				if (!(item.MovementType == (int)AttMovementType.Out && PreviousType == (int)AttMovementType.In))
				{
					TimeSpan ts = item.MovementTime - PreviousMovementTime;
					breaktime = breaktime + ts.TotalMinutes;
					NoOffBreaks++;
				}
				PreviousType = item.MovementType;
				PreviousMovementTime = item.MovementTime;
			}
			//attendance.BreakTime = (int)break time;
			//attendance.Breaks = NoOffBreaks;
			attendance.BreakTime = 0;
			attendance.Breaks = 0;

			return attendance;
		}
		public async Task LeaveToPresentUpdateLeave(ApplyLeave appliedLeave)
		{

			//Updating the leaveBalane if employee present on leave applied day

			LeaveBalance lbalance = new()
			{
				ID = Guid.NewGuid(),
				Type = (int)LeaveTypesScreens.LeaveRevisedFromAttendance,
				Leaves = 1
			};

			if (appliedLeave != null)//if employee Applied
			{
				LeaveBalance leavebalance = await _UOW.GetRepositoryAsync<LeaveBalance>().SingleAsync(l => l.ApplyLeaveId == appliedLeave.ID);
				lbalance.ApplyLeaveId = leavebalance.ApplyLeaveId;
				lbalance.LeaveTypeId = leavebalance.LeaveTypeId;
				lbalance.LeavesAddedOn = leavebalance.LeavesAddedOn;
				lbalance.EmployeeId = leavebalance.EmployeeId;
				appliedLeave.NoOfLeaves--;
				_UOW.GetRepositoryAsync<ApplyLeave>().UpdateAsync(appliedLeave);//Updating the leave balance if employee came to office when appliedLeave date
			}

			await _UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(lbalance);
		}

		public Attendance MissedAttendanceCalculation(Attendance attendance, Employee employee,
			bool isEmpHaveExemptionFromHoliday, ScheduleDetails schedule)
		{
			//Attendance record missed for that day adding the status

			#region Schedule Adding
			attendance = UpdateSchedule(attendance, schedule);
			attendance.Breaks = 0;
			attendance.WorkTime = 0;
			#endregion
			//getting LeaveApplied details
			var empAppliedLeaves = Leaves.Where(a => a.ApplyLeave.EmployeeId == employee.ID);

			//if leave applied then considering as leave
			if (empAppliedLeaves.Any())
			{
				// checking is the day is holiday
				if (Isholiday != null && Isholiday.ReprApplication.Equals("1", StringComparison.OrdinalIgnoreCase) && !isEmpHaveExemptionFromHoliday)
				{
					attendance.AttendanceStatus = (int)AttendanceStatus.Holiday;
				}
				else
				{
					foreach (var leave in empAppliedLeaves)// there is a chance for two leave records with two half days
					{
						if (leave.LeaveCount== 1m)
						{
							attendance.AttendanceStatus = (int)AttendanceStatus.Leave;
							attendance.Leave = 1;
							attendance.LeaveTypeID = leave.LeaveTypeId;
							break;
						}
						else if (leave.LeaveCount == 0.5m)
						{
							attendance.LeaveTypeID = leave.LeaveTypeId;
							ApplyWfh item = GetWFHStatus(employee);//Half day leave with Half Day WFH
							var isHalfDayWFH = false;
							var isFirstHalfWFH = false;
							if (item != null)
							{
								if (attendance.AttendanceDate.Date == item.FromDateC.Date && item.FromHalf)
								{
									isHalfDayWFH = true;
									isFirstHalfWFH = true;
								}
								else if (attendance.AttendanceDate.Date == item.ToDateC.Date && item.ToHalf)
								{
									isHalfDayWFH = true;
									isFirstHalfWFH = false;
								}
							}
							attendance.IsHalfDay = true;
							if (isHalfDayWFH)
							{
								if (isFirstHalfWFH)
								{
									attendance.AttendanceStatus = (int)AttendanceStatus.HalfDayWFH;
									attendance.WFH = 0.5m;
									attendance.HalfDayType = (int)AttendanceStatus.HalfDayLeave;
									attendance.Leave = 0.5m;
								}
								else
								{
									attendance.AttendanceStatus = (int)AttendanceStatus.HalfDayLeave;
									attendance.WFH = 0.5m;
									attendance.HalfDayType = (int)AttendanceStatus.HalfDayWFH;
									attendance.Leave = 0.5m;
								}
								break;
							}
							else// Emp having two leave types with same day
							{
								attendance.Leave = attendance.Leave ??0;
								attendance.AttendanceStatus = (int)AttendanceStatus.HalfDayLeave;
								attendance.Leave +=  0.5m;
							}
							
						}
					}
					if (attendance.Leave == 0.5m && attendance.WFH != 0.5m)
					{
						attendance.HalfDayType = (int)AttendanceStatus.HalfDayAbsent;
						attendance.Absent = 0.5m;
					}
					else if (attendance.Leave== 1 && attendance.AttendanceStatus == (int)AttendanceStatus.HalfDayLeave)// Emp having two leave types with same day
					{
						attendance.HalfDayType = (int)AttendanceStatus.HalfDayLeave;
					}

				}
			}
			else if (Isholiday != null && !isEmpHaveExemptionFromHoliday)
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.Holiday;
			}
			else if (GetWeekOffDetails(employee, attendance.AttendanceDate))//getting week off details
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.WeekOff;
			}
			else if (GetClientVisitStatus(employee))
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.Present;
				attendance.Present = 1;
				attendance.WorkTime = attendance.SchWorkTime;
				attendance.LoginType = (int)LoginType.ClientVisit;
			}
			else if (GetWFHStatus(employee)!=null) //checking WFH option. emp.WorkFromHome == 1
			{
				ApplyWfh item = GetWFHStatus(employee);


				var isHalfDay = false;
				var isFirstHalf = false;
				if (attendance.AttendanceDate.Date == item.FromDateC.Date && item.FromHalf)
				{
					isHalfDay = true;
				
				}
				else if (attendance.AttendanceDate.Date == item.ToDateC.Date && item.ToHalf)
				{ 
					isHalfDay = true;
				}

				if ((item.ToDateC.Date - item.FromDateC.Date).Days > 0)
				{
					if (attendance.AttendanceDate.Date == item.FromDateC.Date)
					{ isFirstHalf = !item.FromHalf; }
					else if (attendance.AttendanceDate.Date == item.ToDateC.Date)
					{ isFirstHalf = item.ToHalf; }
				}
				else
				{ isFirstHalf = item.FromHalf; }

				if (isHalfDay)
				{
					
					attendance.AttendanceStatus = isFirstHalf ? (int)AttendanceStatus.HalfDayWFH : (int)AttendanceStatus.HalfDayAbsent;
					attendance.WFH = 0.5m;
					attendance.HalfDayType= !isFirstHalf ? (int)AttendanceStatus.HalfDayWFH : (int)AttendanceStatus.HalfDayAbsent;
					attendance.Absent = 0.5m;
					attendance.IsHalfDay = true;
				}
				else
				{
					attendance.AttendanceStatus = (int)AttendanceStatus.WFH;
					attendance.WFH = 1;
				}
				attendance.WorkTime = attendance.SchWorkTime;
			}
			else
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.Absent;
				attendance.Absent = 1;
			}

			return attendance;
		}

		public static int GetWeekNumber(DateTime date)
		{
			CultureInfo myCI = new("en-US");
			Calendar myCal = myCI.Calendar;
			CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
			DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
			return myCal.GetWeekOfYear(date, myCWR, myFirstDOW);
		}

		public bool IsEmpHaveExemptionFromHoliday(Employee employee)
		{
			if (Exemption == null)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(Exemption.Employees)
				&& Exemption.Employees.Split(",").Any(x => x.Equals(employee.ID.ToString(), StringComparison.OrdinalIgnoreCase)))
				return true;
			else if (!string.IsNullOrEmpty(Exemption.Teams)
				&& Exemption.Teams.Split(",").Any(x => x.Equals(employee.TeamId.ToString(), StringComparison.OrdinalIgnoreCase)))
				return true;
			else if (!string.IsNullOrEmpty(Exemption.Designations)
				&& Exemption.Designations.Split(",").Any(x => x.Equals(employee.DesignationId.ToString(), StringComparison.OrdinalIgnoreCase)))
				return true;
			else if (!string.IsNullOrEmpty(Exemption.Department)
				&& Exemption.Department.Split(",").Any(x => x.Equals(employee.DepartmentId.ToString(), StringComparison.OrdinalIgnoreCase)))
				return true;
			else if (!string.IsNullOrEmpty(Exemption.Location)
				&& Exemption.Location.Split(",").Any(x => x.Equals(employee.WorkLocationId.ToString(), StringComparison.OrdinalIgnoreCase)))
				return true;
			return false;
		}

		public bool GetWeekOffDetails(Employee employee, DateTime attendanceDate)
		{
			Guid? weekOfId = null;
			var empWise = Allocations.FirstOrDefault(x => x.EmployeeId == employee.ID);
			if (empWise != null && empWise.WeekOffSetupId.HasValue)
			{
				weekOfId = empWise.WeekOffSetupId.Value;
			} 
			else
			{
				weekOfId = employee.Team.WeekOffSetupId
					?? employee.Designation.WeekOffSetupId
					?? employee.Department.WeekOffSetupId
					?? employee.WorkLocation.WeekOffSetupId;
			}
			return weekOfId.HasValue && EmpHaveWeekOff(attendanceDate, weekOfId.Value);
		}

		public bool EmpHaveWeekOff(DateTime attendanceDate, Guid weekOffsetUpId)
		{
			bool isWeekOff = false;
			var weedays = WeekOffDays.Where(x => x.WeekOffSetupId == weekOffsetUpId);
			List<WeekOffDays> weekDays = weedays.ToList();

			if (weekDays.Count > 0)
			{
				foreach (var weekDay in weekDays)
				{
					//Week Date wise
					if (weekDay.Type == 3)
					{
						if (weekDay.WeekDate.Value.Date == attendanceDate.Date)
						{
							return weekDay.Status != 1;
						}
					}
					else if (weekDay.Type == 1)//Monthly week-offs
					{
						if (weekDay.WeekDay.HasValue
							&& (int)attendanceDate.DayOfWeek == weekDay.WeekDay.Value)
						{
							foreach (string weekNo in weekDay.WeekNoInMonth.Split(","))
							{
								DateTime day = DayIsWeekOff(attendanceDate, Convert.ToInt32(weekNo), attendanceDate.DayOfWeek);
								if (day.Date == attendanceDate.Date)
								{
									isWeekOff = true;
								}
							}
						}
					}
					else if (weekDay.Type == 2 && attendanceDate.DayNoInWeek() == (int)weekDay.WeekDay)//yearly week offs
					{
						int weekno = GetWeekNumber(attendanceDate);
						if (weekDay.WeekInYear == 1)//Even Week Offs
						{
							if (weekno % 2 == 0)
								isWeekOff = true;
						}
						else if (weekDay.WeekInYear == 2)//Odd week Off
						{
							if (weekno % 2 != 0)
								isWeekOff = true;
						}
						else if (weekDay.WeekInYear == null || weekDay.WeekInYear == 0) // All Weeks
						{
							isWeekOff = true;
						}
					}
				}
			}
			return isWeekOff;
		}

		public static DateTime DayIsWeekOff(DateTime attendanceDate, int occurrence, DayOfWeek dayOfWeek)
		{
			var wday = new DateTime(attendanceDate.Year, attendanceDate.Month, 1);

			DateTime firstOccurrence = wday.DayOfWeek == dayOfWeek ? wday : wday.AddDays(dayOfWeek - wday.DayOfWeek);
			if (firstOccurrence.Month < attendanceDate.Month)
			{
				occurrence++;
			}
			firstOccurrence = firstOccurrence.AddDays(7 * (occurrence - 1));
			return firstOccurrence;
		}

		public ApplyWfh GetWFHStatus(Employee employee)
		{
			return WFH.FirstOrDefault(x => x.EmployeeId == employee.ID);
		}
		public bool GetClientVisitStatus(Employee employee)
		{
			return ClientVisits.FirstOrDefault(x => x.EmployeeId == employee.ID) != null;
		}
		public WorkHoursSetting GetWorkHours(Employee employee)
		{
			Guid? workHourSettingId = null;
			var empWise = Allocations.FirstOrDefault(x => x.EmployeeId == employee.ID);
			if (empWise != null && empWise.WorkHoursSettingId.HasValue)
			{
				workHourSettingId = empWise.WorkHoursSettingId.Value;
			} 
			else
			{
				workHourSettingId = employee.Team.WorkHoursSettingId
					?? employee.Designation.WorkHoursSettingId
					?? employee.Department.WorkHoursSettingId
					?? employee.WorkLocation.WorkHoursSettingId;
			}
			return workHourSettingId.HasValue ? WorkHoursSettings.FirstOrDefault(x => x.ID == workHourSettingId) : null;
		}
		public Attendance AttBasedOnWorkHours(Attendance attendance, Employee employee)
		{
			var workHourSetting = GetWorkHours(employee);
			if (workHourSetting == null)
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.Present;
				attendance.Present = 1;
				return attendance;
			}

			if (attendance.WorkTime >= workHourSetting.FullDayMinutes)
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.Present;
				attendance.Present = 1;
			}
			else if (attendance.WorkTime >= workHourSetting.HalfDayMinutes)
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.HalfDayPresent;
				attendance.Present = 0.5m;
				attendance.HalfDayType = (int)AttendanceStatus.HalfDayAbsent;
				attendance.Absent = 0.5m;
				attendance.IsHalfDay= true;
			}
			else
			{
				attendance.AttendanceStatus = (int)AttendanceStatus.Absent;
				attendance.Absent = 1;
			}
			return attendance;
		}

	}
}
