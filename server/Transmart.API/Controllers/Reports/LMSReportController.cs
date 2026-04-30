using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Reports;
using TranSmart.Service.Reports;
using TranSmart.Service.Reports.LMS;

namespace TranSmart.API.Controllers.Reports
{
	[Route("api/[controller]")]
	[ApiController]
	public class LmsReportController : BaseController
	{
		private readonly ILmsReportService _service;
		private readonly ILmsService _LMSservice;
		public LmsReportController(ILmsReportService service, ILmsService LMSservice)
		{
			_service = service;
			_LMSservice = LMSservice;
		}
		[HttpGet("AllActiveEmployees")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_ActiveEmployees)]
		public async Task<IActionResult> AllActiveEmplyees([FromQuery] ActiveEmployeeRptModel model)
		{
			var result = new Result<Employee>();
			if (model.FromDate.HasValue && model.ToDate.HasValue)
			{
				return Ok(await _service.AllActiveEmployees(model));

			}
			result.AddMessageItem(new MessageItem("Please select date of joining from and to."));
			return BadRequest(result);
		}

		[HttpGet("Attendance")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_Attendance)]
		public async Task<IActionResult> Attendances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId,
										DateTime? fromDate, DateTime? toDate)
		{
			var result = new Result<TranSmart.Domain.Entities.Leave.Attendance>();
			if (fromDate.HasValue && toDate.HasValue)
			{
				return Ok(await _service.Attendances(departmentId, designationId, teamId, employeeId, fromDate, toDate));
			}
			result.AddMessageItem(new MessageItem("Please select attendance date from and to."));
			return BadRequest(result);

		}

		[HttpGet("AllEmployeeContactDetails")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_ContactDetails)]
		public async Task<IActionResult> AllEmployeeContactDetails(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate)
		{
			var result = new Result<EmployeeFamily>();
			if (fromDate.HasValue || toDate.HasValue)
			{
				return Ok(await _service.AllEmployeeContactDetails(departmentId, designationId, teamId, employeeId, fromDate, toDate));
			}
			result.AddMessageItem(new MessageItem("Please select date of joining from and to."));
			return BadRequest(result);

		}

		[HttpGet("EmployeeActiveAddress")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_EmployeeAddress)]
		public async Task<IActionResult> EmployeeAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type)
		{
			var result = new Result<EmployeeEmergencyAd>();
			if (joinDateFrom.HasValue && joinDateTo.HasValue && type.HasValue)
			{
				switch (type)
				{
					case 1:
						return Ok(await _service.EmployeePresentAddress(departmentId, designationId, teamId, employeeId, joinDateFrom, joinDateTo, type));
					case 2:
						return Ok(await _service.EmployeePermanentAddress(departmentId, designationId, teamId, employeeId, joinDateFrom, joinDateTo, type));
					case 3:
						return Ok(await _service.EmployeeEmergencyAddress(departmentId, designationId, teamId, employeeId, joinDateFrom, joinDateTo, type));
				}
			}
			result.AddMessageItem(new MessageItem("Please select date of joining from and to."));
			return BadRequest(result);
		}

		[HttpGet("LeaveBalance")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_LeaveBalances)]
		public async Task<IActionResult> LeaveBalance(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? leaveTypeId)
		{
			return Ok(await _LMSservice.LeaveBalances(departmentId, designationId, teamId, employeeId, leaveTypeId));
		}
		
		[HttpGet("MyTeamAttendance")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_MyTeamAttendance)]
		public async Task<IActionResult> MyTeamAttendance(Guid? designationId, Guid? employeeId, DateTime? fromDate, DateTime? toDate, int? attendanceStatus)
		{
			var result = new Result<TranSmart.Domain.Entities.Leave.Attendance>();
			if ((designationId.HasValue || employeeId.HasValue) &&
				(fromDate.HasValue || toDate.HasValue))
			{
				return Ok(await _service.MyTeamAttendance(designationId, employeeId, fromDate, toDate, attendanceStatus, LOGIN_USER_EMPId));
			}
			result.AddMessageItem(new MessageItem("Select anyone from Designation, Employee, and Attendance From Date or Attendance To Date"));
			return BadRequest(result);

		}

		[HttpGet("MyDepartmentAttendance")]
		public async Task<IActionResult> MyDepartmentAttendance(Guid? lobId, Guid? functionalAreaId,
			Guid? designationId, DateTime? fromDate, DateTime? toDate, int? employeeStatus)
		{
			var result = new Result<TranSmart.Domain.Entities.Leave.Attendance>();
			return Ok(await _service.MyDepartmentAttendance(lobId, functionalAreaId, designationId, fromDate, toDate, LOGIN_USER_DEPTID, employeeStatus));
		}
		[HttpGet("RCMAttendance")]
		public async Task<IActionResult> RCMAttendance(Guid? lobId, Guid? functionalAreaId,
			Guid? designationId, DateTime? fromDate, DateTime? toDate, int? employeeStatus)
		{
			return Ok(await _service.RCMAttendance(lobId, functionalAreaId, designationId, fromDate, toDate, employeeStatus));
		}
		[HttpGet("MyTeamLeaveBalances")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_MyTeamLeaveBalance)]
		public async Task<IActionResult> MyTeamLeaveBalances(Guid? designationId, Guid? employeeId, Guid? leaveTypeId)
		{
			return Ok(await _service.MyTeamLeaveBalances(designationId, employeeId, leaveTypeId, LOGIN_USER_EMPId));
		}
		[HttpGet("LeaveBalanceDetails")]
		public async Task<IActionResult> LeaveBalanceDetails([FromQuery] BaseSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(await _service.LeaveBalanceDetails(baseSearch));
		}

		[HttpGet("ResignedEmployees")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_ResignedEmployees)]
		public async Task<IActionResult> ResignedEmployees(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate)
		{
			var result = new Result<Employee>();
			if ((departmentId.HasValue || designationId.HasValue || teamId.HasValue || employeeId.HasValue) &&
				(fromDate.HasValue || toDate.HasValue))
			{
				return Ok(await _service.ResignedEmployees(departmentId, designationId, teamId, employeeId, fromDate, toDate));
			}
			result.AddMessageItem(new MessageItem("Select anyone from Department, Designation, Team , Employee, and Resigned From Date or Resigned To Date"));
			return BadRequest(result);

		}

		[HttpGet("ResignedDepartmentEmployees")]
		public async Task<IActionResult> ResignedDepartmentEmployees(Guid? lineOfbusinessId, DateTime? fromDate, DateTime? toDate)
		{
			var result = new Result<Employee>();
			if (fromDate.HasValue && toDate.HasValue)
			{
				return Ok(await _service.ResignedDepartmentEmployees(LOGIN_USER_DEPTID, lineOfbusinessId, fromDate, toDate));
			}
			result.AddMessageItem(new MessageItem("Select resigned from date and resigned to date"));
			return BadRequest(result);

		}

		[HttpGet("WeekOffSetup")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_EmployeeWeekOff)]
		public async Task<IActionResult> WeekOffSetup(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? weekOffId)
		{
			Result<WeekOffSetup> result = new Result<WeekOffSetup>();
			if (departmentId.HasValue || designationId.HasValue || teamId.HasValue || employeeId.HasValue || weekOffId.HasValue)
			{
				return Ok(await _service.WeekOffReport(departmentId, designationId, teamId, employeeId, weekOffId));
			}
			result.AddMessageItem(new MessageItem("Select anyone from Department, Designation, Team , Employee, or Week Off"));
			return BadRequest(result);

		}
		[HttpGet("Shift")]
		[ApiReportAuthorize(Core.ReportPermission.LMS_EmployeeShift)]
		public async Task<IActionResult> Shift(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? shiftId)
		{
			Result<Shift> result = new Result<Shift>();
			if (departmentId.HasValue || designationId.HasValue || teamId.HasValue || employeeId.HasValue || shiftId.HasValue)
			{
				return Ok(await _service.ShiftReport(departmentId, designationId, teamId, employeeId, shiftId));
			}
			result.AddMessageItem(new MessageItem("Select anyone from Department, Designation, Team , Employee, or Shift"));
			return BadRequest(result);
		}

		[HttpGet("SelfAttendance")]
		[ApiAuthorize(Core.Permission.SS_Attendance, Core.Privilege.Read)]
		public async Task<IActionResult> SelfAttendance(DateTime? fromDate, DateTime? toDate, int? attendanceStatus)
		{
			Result<TranSmart.Domain.Entities.Leave.Attendance> result = new Result<TranSmart.Domain.Entities.Leave.Attendance>();
			if (!fromDate.HasValue && !toDate.HasValue && attendanceStatus == null)
			{
				result.AddMessageItem(new MessageItem("From Date, To Date and Status are required"));
				return BadRequest(result);
			}
			return Ok(await _service.SelfAttendance(fromDate, toDate, attendanceStatus, LOGIN_USER_EMPId));
		}
		[HttpGet("ReportingToAttendance")]
		public async Task<IActionResult> ReportingToAttendance(Guid? employeeId, DateTime? fromDate, DateTime? toDate, int? attendanceStatus)
		{
			Result<TranSmart.Domain.Entities.Leave.Attendance> result = new Result<TranSmart.Domain.Entities.Leave.Attendance>();
			if (!fromDate.HasValue && !toDate.HasValue && attendanceStatus == null)
			{
				result.AddMessageItem(new MessageItem("From Date, To Date and Status are required"));
				return BadRequest(result);
			}
			return Ok(await _service.ReportingToAttendance(employeeId, fromDate, toDate, attendanceStatus, LOGIN_USER_EMPId));
		}
		[HttpGet("ReportingToEmployee")]
		public async Task<IActionResult> ReportingToEmployee(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? reportingToId)
		{
			return Ok(await _service.ReportingToEmployee(departmentId, designationId, teamId, employeeId, reportingToId));
		}
		[HttpGet("EmployeeLOB")]
		public async Task<IActionResult> EmployeeLOBUnique()
		{
			return Ok(await _service.EmployeeLOBUnique(LOGIN_USER_DEPTID));
		}
		[HttpGet("FunctionalArea")]
		public async Task<IActionResult> FunctionalArea([FromQuery] Guid? lobId)
		{
			return Ok(await _service.FunctionalAreaUnique(LOGIN_USER_DEPTID, lobId));
		}
		[HttpGet("Designations")]
		public async Task<IActionResult> DesignationsUnique([FromQuery] Guid? lobId, [FromQuery] Guid? functionalAreaId)
		{
			return Ok(await _service.DesignationsUnique(LOGIN_USER_DEPTID, lobId, functionalAreaId));
		}
		[HttpGet("Pages")]
		public async Task<IActionResult> PagesUnique([FromQuery] Guid moduleId)
		{
			return Ok(await _service.PagesUnique(moduleId));
		}
		[HttpGet("PageEmployee")]
		public async Task<IActionResult> PageEmployees(Guid? moduleId, Guid? pageId)
		{
			var result = new Result<User>();
			if (moduleId == null || pageId == null)
			{
				result.AddMessageItem(new MessageItem(ErrMessages.Module_And_Page_Are_Required));
				return BadRequest(result);
			}
			return Ok(await _service.PageEmployees(pageId));
		}
		[HttpGet("PageRole")]
		public async Task<IActionResult> PageRole(Guid? moduleId, Guid? pageId)
		{
			return Ok(await _service.PageRole(moduleId, pageId));
		}
	}
}
