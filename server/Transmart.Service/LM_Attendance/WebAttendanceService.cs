using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.LM_Attendance;
using TranSmart.Domain.Models.LM_Attendance.Request;
using TranSmart.Service.Leave;
using TranSmart.Service.Schedules;

namespace TranSmart.Service.LM_Attendance
{
	public interface IWebAttendanceService : IBaseService<WebAttendance>
	{
		Task<Result<WebAttendance>> UpdateTimings(Guid employeeId);
		Task<Result<WebAttendance>> RePunchIn(Guid empId);
		Task<WebAttendance> GetPunchIn(Guid EmpId);
		Task<bool> IsPunchEmployee(Guid empId);
		Task<WebAttendance> GetWebAttendance(Guid id, Guid approverId);
		Task<Result<WebAttendance>> ApprovalUpdate(WebAttendanceRequest model);
		Task<Result<WebAttendance>> Reject(WebAttendanceRequest model);
	}
	public class WebAttendanceService : BaseService<WebAttendance>, IWebAttendanceService
	{
		private readonly IAttendanceService _attendanceService;
		public WebAttendanceService(IUnitOfWork uow, IAttendanceService attendanceService) : base(uow)
		{
			_attendanceService = attendanceService;
		}
		public override async Task<Result<WebAttendance>> AddAsync(WebAttendance item)
		{
			var result = new Result<WebAttendance>();
			var webAttendance = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.EmployeeId == item.EmployeeId
								&& x.AttendanceDate.Date == DateTime.Now.Date && x.Status == (int)WebAttendanceSts.Approved);
			if (webAttendance != null)
			{
				result.AddMessageItem(new MessageItem("Already web attendance approved"));
				return result;
			}
			var employee = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == item.EmployeeId);
			item.AttendanceDate = DateTime.Now;
			item.InTime = DateTime.Now;
			item.OutTime = null;
			item.Status = (int)WebAttendanceSts.Applied;
			return await base.AddAsync(item);
		}
		public async Task<Result<WebAttendance>> UpdateTimings(Guid employeeId)
		{
			Result<WebAttendance> result = new();
			var attendance = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.EmployeeId == employeeId
													&& x.AttendanceDate.Date == DateTime.Today && x.Status == (int)WebAttendanceSts.Applied);
			if (attendance != null)
			{
				attendance.OutTime = DateTime.Now;
				result = await base.UpdateAsync(attendance);
				return result;
			}
			result.AddMessageItem(new MessageItem("Invalid Data"));
			return result;
		}
		public async Task<Result<WebAttendance>> RePunchIn(Guid empId)
		{
			Result<WebAttendance> result = new();
			var attendance = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.EmployeeId == empId
										&& x.AttendanceDate.Date == DateTime.Today && x.Status == (int)WebAttendanceSts.Applied);
			if (attendance != null)
			{
				attendance.InTime = DateTime.Now;
				attendance.OutTime = null;
				result = await base.UpdateAsync(attendance);
				return result;
			}
			result.AddMessageItem(new MessageItem("Invalid Data"));
			return result;
		}

		public async Task<WebAttendance> GetPunchIn(Guid EmpId)
		{
			var result = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.EmployeeId == EmpId && x.AttendanceDate.Date == DateTime.Today
																					&& x.Status == (int)WebAttendanceSts.Applied,
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

		public override async Task<IPaginate<WebAttendance>> GetPaginate(BaseSearch baseSearch)
		{
			WebAttendanceSearch search = (WebAttendanceSearch)baseSearch;
			search.Status = search.Status == 0 ? (int)WebAttendanceSts.Applied : search.Status;
			return await UOW.GetRepositoryAsync<WebAttendance>().GetPageListAsync(
				predicate: x => x.Status == search.Status && x.OutTime.HasValue,
				//x.Status == (int)WebAttendanceSts.Applied && x.OutTime.HasValue,
				include: x => x.Include(i => i.Employee).ThenInclude(x => x.Designation)
								.Include(i => i.Employee).ThenInclude(x => x.Department),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Employee.Name", ascending: !baseSearch.IsDescend);
		}

		public async Task<WebAttendance> GetWebAttendance(Guid id, Guid approverId)
		{
			return await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.ID == id,
					//&& (x.Employee.ReportingToId == approverId || x.Employee.ReportingTo.ReportingToId == approverId),
					include: x => x.Include(i => i.Employee).ThenInclude(x => x.ReportingTo)
									.Include(i => i.Employee).ThenInclude(x => x.Designation));
		}

		public async Task<Result<WebAttendance>> ApprovalUpdate(WebAttendanceRequest model)
		{
			Result<WebAttendance> result = new();
			var webAttendance = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.ID == model.ID && x.Status == (int)WebAttendanceSts.Applied,
																							include: i => i.Include(x => x.Employee));
			if (webAttendance != null)
			{
				webAttendance.Status = (int)WebAttendanceSts.Approved;
				webAttendance.ApprovedById = model.ApprovedById;
				var existingAttendance = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == model.EmployeeId && x.AttendanceDate == webAttendance.AttendanceDate);
				if (existingAttendance != null)
				{
					existingAttendance.AttendanceStatus = (int)AttendanceStatus.Present;
					existingAttendance.Present = 1;
					existingAttendance.Absent = null;
					existingAttendance.UADays = null;
					existingAttendance.LeaveTypeID = null;
					existingAttendance.IsHalfDay = null;
					existingAttendance.HalfDayType = null;
					existingAttendance.IsFirstHalf = null;
					existingAttendance.Leave = null;
					existingAttendance.WFH = null;
					existingAttendance.LoginType = (int)LoginType.WebPunch;
					existingAttendance.InTime = webAttendance.InTime;
					existingAttendance.OutTime = webAttendance.OutTime;

					UOW.GetRepositoryAsync<Attendance>().UpdateAsync(existingAttendance);
					await UOW.SaveChangesAsync();
				}
				var attendance = new Attendance
				{
					EmployeeId = model.EmployeeId,
					AttendanceDate = webAttendance.AttendanceDate,
					InTime = webAttendance.InTime,
					OutTime = webAttendance.OutTime,
					AttendanceStatus = (int)AttendanceStatus.Present,
					LoginType = (int)LoginType.WebPunch,
				};
				await UOW.GetRepositoryAsync<Attendance>().AddAsync(attendance);
				var bioAttendance = new List<BiometricAttLogs>
				{
					new BiometricAttLogs
					{
						EmployeeId = model.EmployeeId,
						EmpCode = webAttendance.Employee.No.ToUpper().Replace("AVONTIX", ""),
						MovementTime = webAttendance.InTime,
						MovementType = 0,
						AttendanceDate = webAttendance.AttendanceDate,
						Type = (byte)AttendanceType.Web
					},
					new BiometricAttLogs
					{
						EmployeeId = model.EmployeeId,
						EmpCode = webAttendance.Employee.No.ToUpper().Replace("AVONTIX", ""),
						MovementTime = (DateTime)webAttendance.OutTime,
						MovementType = 1,
						AttendanceDate = webAttendance.AttendanceDate,
						Type = (byte)AttendanceType.Web
					}
				};

				//await UOW.GetRepositoryAsync<BiometricAttLogs>().AddAsync(bioAttendance);
				var biometricLogs = await _attendanceService.BiometricLogsImport(bioAttendance);
				if (biometricLogs.HasError)
				{
					result.AddMessageItem(biometricLogs.Messages.First());
					return result;
				}
				result = await base.UpdateAsync(webAttendance);
				return result;
			}
			result.AddMessageItem(new MessageItem("Invalid Data"));
			return result;
		}
		public async Task<Result<WebAttendance>> Reject(WebAttendanceRequest model)
		{
			Result<WebAttendance> result = new();
			var attendance = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.AttendanceDate.Date == model.AttendanceDate.Date && x.EmployeeId == model.EmployeeId);
			if (attendance != null)
			{
				result.AddMessageItem(new MessageItem("Attendance already executed"));
				return result;
			}
			var webAttendance = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.ID == model.ID && x.Status != (int)WebAttendanceSts.Rejected);
			if (webAttendance != null)
			{
				webAttendance.Status = (int)WebAttendanceSts.Rejected;
				webAttendance.ApprovedById = model.ApprovedById;
				webAttendance.RejectReason = model.RejectReason;
				result = await base.UpdateAsync(webAttendance);
				return result;
			}
			result.AddMessageItem(new MessageItem("Invalid Data"));
			return result;

		}
	}
}
