using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository.Leave;
using TranSmart.Data.Repository.Reports;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.Reports;
using TranSmart.Domain.Models.Reports.Generated;
using TranSmart.Domain.Models.Reports.LMS;
using TranSmart.Domain.Reports;

namespace TranSmart.Service.Reports
{
	public interface ILmsReportService
	{
		Task<IEnumerable<AttendanceModel>> Attendances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate);
		Task<IEnumerable<AttendanceModel>> MyTeamAttendance(Guid? designationId, Guid? employeeId, DateTime? fromDate, DateTime? toDate, int? attendanceStatus, Guid ReportingToId);
		Task<IEnumerable<AttendanceModel>> MyDepartmentAttendance(Guid? lobId, Guid? functionalAreaId, Guid? designationId, DateTime? fromDate, DateTime? toDate, Guid departmentId, int? employeeStatus);
		Task<IEnumerable<EmployeeResignationModel>> ResignedEmployees(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate);
		Task<IEnumerable<ResignationDepartmentModel>> ResignedDepartmentEmployees(Guid? departmentId, Guid? lobId, DateTime? fromDate, DateTime? toDate);
		Task<IEnumerable<ShiftModel>> WeekOffReport(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? weekOffId);
		Task<IEnumerable<EmployeeActiveAddressModel>> EmployeePermanentAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type);
		Task<IEnumerable<EmployeeActiveAddressModel>> EmployeePresentAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type);
		Task<IEnumerable<EmployeeActiveAddressModel>> EmployeeEmergencyAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type);
		Task<IEnumerable<AllActiveEmployeesReportModel>> AllActiveEmployees(ActiveEmployeeRptModel model);
		Task<IEnumerable<AttendanceReportModel>> SelfAttendance(DateTime? fromDate, DateTime? toDate, int? attendanceStatus, Guid empId);
		Task<IEnumerable<EmployeeContactReportModel>> AllEmployeeContactDetails(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate);
		Task<IEnumerable<LeaveBalancesModel>> MyTeamLeaveBalances(Guid? designationId, Guid? employeeId, Guid? leaveTypeId, Guid ReportingToId);
		Task<IEnumerable<LeaveBalancesModel>> LeaveBalances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? leaveTypeId);
		Task<IEnumerable<ShiftModel>> ShiftReport(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? shiftId);
		Task<IEnumerable<ReportingEmployeeModel>> ReportingToEmployee(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? reportingToId);
		Task<IEnumerable<LOBModel>> EmployeeLOBUnique(Guid departmentId);
		Task<IEnumerable<FunctionalAreaModel>> FunctionalAreaUnique(Guid departmentId, Guid? lobId);
		Task<IEnumerable<DesignationModel>> DesignationsUnique(Guid departmentId, Guid? lobId, Guid? functionalAreaId);
		Task<IEnumerable<AttendanceModel>> RCMAttendance(Guid? lobId, Guid? functionalAreaId, Guid? designationId, DateTime? fromDate, DateTime? toDate, int? employeeStatus);
		Task<IEnumerable<TranSmart.Domain.Entities.Page>> PagesUnique(Guid moduleId);
		Task<IEnumerable<UserReportModel>> PageEmployees(Guid? pageId);
		Task<IEnumerable<RoleReportModel>> PageRole(Guid? moduleId, Guid? pageId);
		Task<IEnumerable<AttendanceReportModel>> ReportingToAttendance(Guid? employeeId, DateTime? fromDate, DateTime? toDate, int? attendanceStatus, Guid empId);
		Task<IPaginate<LeaveBalanceDetailsModel>> LeaveBalanceDetails(BaseSearch search);
	}
	public class LmsReportService : ILmsReportService
	{
		private readonly IUnitOfWork _UOW;
		private readonly ILeaveBalanceRepository _repository;
		private readonly IRoleRepository _roleRepository;
		public LmsReportService(IUnitOfWork uow
			, ILeaveBalanceRepository repository, IRoleRepository roleRepository)
		{
			_UOW = uow;
			_repository = repository;
			_roleRepository = roleRepository;
		}
		public static string AttendenceStatus(int? status)
		{
			return ((AttendanceStatus)status).ToString();
		}
		public async Task<IEnumerable<AttendanceModel>> Attendances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate)
		{
			return await _UOW.GetRepositoryAsync<Attendance>().GetWithSelectAsync(
			  selector: d => new AttendanceReportModel
			  {
				  AttendanceStatus = d.AttendanceStatus,
				  Name = d.Employee.Name,
				  Date = d.AttendanceDate,
				  PunchIn = d.InTime,
				  PunchOut = d.OutTime,
				  BreakTime = d.BreakTime,
				  WorkTime = d.WorkTime,
				  Status = AttendenceStatus(d.AttendanceStatus)
			  },
			predicate: x => (((!fromDate.HasValue || x.AttendanceDate >= fromDate)
								&& (!toDate.HasValue || x.AttendanceDate <= toDate))
								&& ((departmentId == null || x.Employee.DepartmentId == departmentId)
								&& (designationId == null || x.Employee.DesignationId == designationId)
								&& (employeeId == null || x.EmployeeId == employeeId)
								&& (teamId == null || x.Employee.TeamId == teamId))),
			include: i => i.Include(x => x.Employee),
			orderBy: o => o.OrderBy(x => x.Employee.Name).ThenBy(x => x.AttendanceDate));
		}
		public async Task<IEnumerable<AttendanceModel>> MyTeamAttendance(Guid? designationId, Guid? employeeId, DateTime? fromDate, DateTime? toDate, int? attendanceStatus, Guid ReportingToId)
		{
			return await _UOW.GetRepositoryAsync<Attendance>().GetWithSelectAsync(
				selector: d => new AttendanceReportModel
				{
					Name = d.Employee.Name,
					Designation = d.Employee.Designation.Name,
					AttendanceStatus = d.AttendanceStatus,
					Date = d.AttendanceDate,
					PunchIn = d.InTime,
					PunchOut = d.OutTime,
					BreakTime = d.BreakTime,
					WorkTime = d.WorkTime,
					Status = AttendenceStatus(d.AttendanceStatus)
				},
			   predicate: x => (((!fromDate.HasValue || x.AttendanceDate >= fromDate)
								&& (!toDate.HasValue || x.AttendanceDate <= toDate))
								&& ((designationId == null || x.Employee.DesignationId == designationId)
								&& (employeeId == null || x.EmployeeId == employeeId)
								&& (attendanceStatus == null || x.AttendanceStatus == attendanceStatus))) && x.Employee.ReportingToId == ReportingToId,
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
			   orderBy: s => s.OrderBy(x => x.AttendanceDate).ThenBy(x => x.Employee.Name));
		}
		public async Task<IEnumerable<AttendanceReportModel>> ReportingToAttendance(Guid? employeeId, DateTime? fromDate, DateTime? toDate, int? attendanceStatus, Guid reportingToId)
		{
			return await _UOW.GetRepositoryAsync<Attendance>().GetWithSelectAsync(
				include: x => x.Include(i => i.Employee),
				selector: d => new AttendanceReportModel
				{
					EmployeeId = d.EmployeeId,
					Name = d.Employee.Name,
					AttendanceStatus = d.AttendanceStatus,
					Date = d.AttendanceDate,
					PunchIn = d.InTime.Value,
					PunchOut = d.OutTime.Value,
					BreakTime = d.BreakTime,
					WorkTime = d.WorkTime,
					LoginType = d.LoginType,
					Status = AttendenceStatus(d.AttendanceStatus),
				},
				predicate: x => x.Employee.ReportingToId == reportingToId && (employeeId == null || x.EmployeeId == employeeId)
									&& x.AttendanceDate.Date >= fromDate && x.AttendanceDate.Date <= toDate
				  && (attendanceStatus == null || x.AttendanceStatus == attendanceStatus),
				orderBy: o => o.OrderByDescending(x => x.AttendanceDate));
		}
		public async Task<IEnumerable<AttendanceModel>> MyDepartmentAttendance(Guid? lobId, Guid? functionalAreaId, Guid? designationId, DateTime? fromDate, DateTime? toDate, Guid departmentId, int? employeeStatus)
		{
			return await _UOW.GetRepositoryAsync<Attendance>().GetWithSelectAsync(
				selector: d => new AttendanceReportModel
				{
					Name = d.Employee.Name,
					Designation = d.Employee.Designation.Name,
					AttendanceStatus = d.AttendanceStatus,
					Date = d.AttendanceDate,
					PunchIn = d.InTime.Value,
					PunchOut = d.OutTime.Value,
					BreakTime = d.BreakTime,
					WorkTime = d.WorkTime,
					Status = AttendenceStatus(d.AttendanceStatus),
					LOB = d.Employee.LOB.Name,
					FA = d.Employee.FunctionalArea.Name,
				},
			   predicate: x => ((!fromDate.HasValue || x.AttendanceDate >= fromDate)
								&& (!toDate.HasValue || x.AttendanceDate <= toDate))
								&& (lobId == null || x.Employee.LOBId == lobId)
								&& (functionalAreaId == null || x.Employee.FunctionalAreaId == functionalAreaId)
								&& (designationId == null || x.Employee.DesignationId == designationId)
								&& x.Employee.DepartmentId == departmentId
								&& (employeeStatus == null || x.Employee.Status == employeeStatus),
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
			   orderBy: s => s.OrderBy(x => x.Employee.Name).ThenBy(x => x.AttendanceDate));
		}


		public async Task<IEnumerable<AttendanceModel>> RCMAttendance(Guid? lobId, Guid? functionalAreaId, Guid? designationId, DateTime? fromDate, DateTime? toDate, int? employeeStatus)
		{
			return await _UOW.GetRepositoryAsync<Attendance>().GetWithSelectAsync(
				selector: d => new AttendanceReportModel
				{
					Name = d.Employee.Name,
					Department = d.Employee.Department.Name,
					Designation = d.Employee.Designation.Name,
					AttendanceStatus = d.AttendanceStatus,
					Date = d.AttendanceDate,
					PunchIn = d.InTime.Value,
					PunchOut = d.OutTime.Value,
					BreakTime = d.BreakTime,
					WorkTime = d.WorkTime,
					Status = AttendenceStatus(d.AttendanceStatus),
					LOB = d.Employee.LOB.Name,
					FA = d.Employee.FunctionalArea.Name,
				},
			   predicate: x => ((!fromDate.HasValue || x.AttendanceDate >= fromDate)
								&& (!toDate.HasValue || x.AttendanceDate <= toDate))
								&& (lobId == null || x.Employee.LOBId == lobId)
								&& (functionalAreaId == null || x.Employee.FunctionalAreaId == functionalAreaId)
								&& (designationId == null || x.Employee.DesignationId == designationId)
								&& (x.Employee.DepartmentId == Guid.Parse("CADC6D04-7148-46D0-8088-FF524C6F4497")
									|| x.Employee.DepartmentId == Guid.Parse("719a5756-d29f-4020-1378-08db31b7a366")
									|| x.Employee.DepartmentId == Guid.Parse("A0AB894C-7134-45EE-1377-08DB31B7A366"))
								&& (employeeStatus == null || x.Employee.Status == employeeStatus),
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
			   orderBy: s => s.OrderBy(x => x.Employee.Name).ThenBy(x => x.AttendanceDate));
		}


		public async Task<IEnumerable<AttendanceReportModel>> SelfAttendance(DateTime? fromDate, DateTime? toDate, int? attendanceStatus, Guid empId)
		{
			return await _UOW.GetRepositoryAsync<Attendance>().GetWithSelectAsync(
				include: x => x.Include(i => i.Employee),
				selector: d => new AttendanceReportModel
				{
					EmployeeId = d.EmployeeId,
					AttendanceStatus = attendanceStatus.HasValue && attendanceStatus == d.HalfDayType ? d.HalfDayType : d.AttendanceStatus,
					Date = d.AttendanceDate,
					PunchIn = d.InTime.Value,
					PunchOut = d.OutTime.Value,
					BreakTime = d.BreakTime,
					WorkTime = d.WorkTime,
					LoginType = d.LoginType,
					IsHalfDay = d.IsHalfDay,
					HalfDayType = d.HalfDayType,
					Status = AttendenceStatus(d.AttendanceStatus)
					//Status = (d.IsHalfDay != null && d.IsHalfDay == true) ?
					//			string.Format("{0}\n{1}", AttendenceStatus(d.AttendanceStatus), AttendenceStatus(d.HalfDayType))
					//			: ((AttendanceStatus)d.AttendanceStatus).ToString(),
				},
				predicate: x => x.EmployeeId == empId && x.AttendanceDate.Date >= fromDate && x.AttendanceDate.Date <= toDate
				  && (!attendanceStatus.HasValue || (x.IsHalfDay == true ? (x.AttendanceStatus == attendanceStatus || x.HalfDayType == attendanceStatus) : x.AttendanceStatus == attendanceStatus)),
				orderBy: o => o.OrderByDescending(x => x.AttendanceDate));
		}

		public async Task<IEnumerable<EmployeeResignationModel>> ResignedEmployees(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate)
		{
			return await _UOW.GetRepositoryAsync<EmployeeResignation>().GetWithSelectAsync(
				predicate: x => (!fromDate.HasValue || x.ResignationOn.Date >= fromDate)
								 && (!toDate.HasValue || x.ResignationOn.Date <= toDate)
								 && (!departmentId.HasValue || x.Employee.DepartmentId == departmentId)
								 && (!designationId.HasValue || x.Employee.DesignationId == designationId)
								 && (!employeeId.HasValue || x.EmployeeId == employeeId)
								 && (!teamId.HasValue || x.Employee.TeamId == teamId),
				include: x => x.Include(i => i.Employee).ThenInclude(x => x.Department)
				.Include(i => i.Employee).ThenInclude(x => x.Designation)
				.Include(i => i.Employee).ThenInclude(x => x.Team),

				selector: e => new EmployeeResignationModel
				{
					EmployeeName = e.Employee.Name,
					EmployeeNo = e.Employee.No,
					ResignationOn = e.ResignationOn,
					ApprovedOn = e.ApprovedOn,
					LeavingReason = e.LeavingReason,
					LeavingOn = e.Employee.LastWorkingDate,
					ResignationType = e.ResignationType,
					AllowRehiring = e.AllowRehiring,
					Description = e.Description
				});
		}

		public async Task<IEnumerable<ResignationDepartmentModel>> ResignedDepartmentEmployees(Guid? departmentId, Guid? lobId, DateTime? fromDate, DateTime? toDate)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				predicate: x => (!fromDate.HasValue || x.LastWorkingDate >= fromDate)
								 && (!toDate.HasValue || x.LastWorkingDate <= toDate)
								 && (!departmentId.HasValue || x.DepartmentId == departmentId)
								 && (!lobId.HasValue || x.LOBId == lobId),

				selector: e => new ResignationDepartmentModel
				{
					EmployeeName = e.Name,
					EmployeeNo = e.No,
					LOB = e.LOB.Name,
					FunctionalArea = e.FunctionalArea.Name,
					Designation = e.Designation.Name,
					DateOfJoining = e.DateOfJoining,
					LastWorkingDate = e.LastWorkingDate
				});
		}
		public async Task<IEnumerable<ShiftModel>> WeekOffReport(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? weekOffId)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				predicate: p => (!departmentId.HasValue || p.DepartmentId == departmentId)
					&& (!employeeId.HasValue || p.ID == employeeId)
					&& (!designationId.HasValue || p.DesignationId == designationId)
					&& (!teamId.HasValue || p.TeamId == teamId)
					&& (!weekOffId.HasValue || p.Designation.WeekOffSetupId == weekOffId
					|| p.Department.WeekOffSetupId == weekOffId
					|| p.Team.WeekOffSetupId == weekOffId
					|| p.Allocation.WeekOffSetupId == weekOffId),
			   include: i => i.Include(x => x.Department).ThenInclude(x => x.WeekOffSetup)
				.Include(x => x.Designation).ThenInclude(x => x.WeekOffSetup)
				.Include(x => x.Department).ThenInclude(x => x.WeekOffSetup)
				.Include(x => x.Allocation).ThenInclude(x => x.WeekOffSetup),
			   selector: a => new ShiftModel
			   {
				   No = a.No,
				   Name = a.Name,
				   Department = a.Department.Name,
				   Designation = a.Designation.Name,
				   DOJ = a.DateOfJoining,
				   WeekOff = (a.Allocation.WeekOffSetup
					?? a.Team.WeekOffSetup
					?? a.Designation.WeekOffSetup
					?? a.Department.WeekOffSetup ?? new WeekOffSetup { Name = "N/A" }).Name
			   });
		}
		public async Task<IEnumerable<ShiftModel>> ShiftReport(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? shiftId)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				predicate: p => (!employeeId.HasValue || p.ID == employeeId)
							   && (!departmentId.HasValue || p.DepartmentId == departmentId)
							   && (!designationId.HasValue || p.DesignationId == designationId)
							   && (!teamId.HasValue || p.TeamId == teamId)
							   && (!shiftId.HasValue
							   || p.Department.ShiftId == shiftId
							   || p.Designation.ShiftId == shiftId
							   || p.Team.ShiftId == shiftId
							   || p.Allocation.ShiftId == shiftId),
				include: i => i.Include(x => x.Department).ThenInclude(x => x.Shift)
				.Include(x => x.Designation).ThenInclude(x => x.Shift)
				.Include(x => x.Department).ThenInclude(x => x.Shift)
				.Include(x => x.Allocation).ThenInclude(x => x.Shift),
				selector: a => new ShiftModel
				{
					No = a.No,
					Name = a.Name,
					Department = a.Department.Name,
					Designation = a.Designation.Name,
					DOJ = a.DateOfJoining,
					Shift = (a.Allocation.Shift
					?? a.Team.Shift
					?? a.Designation.Shift
					?? a.Department.Shift ?? new Shift { Name = "N/A" }).Name
				});
		}

		public async Task<IEnumerable<EmployeeActiveAddressModel>> EmployeeEmergencyAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type)
		{
			return await _UOW.GetRepositoryAsync<EmployeeEmergencyAd>().GetWithSelectAsync(
			   selector: d => new EmployeeActiveAddressModel
			   {
				   AddressLineOne = d.AddressLineOne,
				   AddressLineTwo = d.AddressLineTwo,
				   CityOrTown = d.CityOrTown,
				   Country = d.Country,
				   State = d.State,
				   EmpName = d.Employee.Name
			   },
			  predicate: x => x.AddressLineOne != "" && (((!joinDateFrom.HasValue || x.Employee.DateOfJoining >= joinDateFrom)
								&& (!joinDateTo.HasValue || x.Employee.DateOfJoining <= joinDateTo))
								&& ((departmentId == null || x.Employee.DepartmentId == departmentId)
								&& (designationId == null || x.Employee.DesignationId == designationId)
								&& (employeeId == null || x.EmployeeId == employeeId)
								&& (teamId == null || x.Employee.TeamId == teamId))),
			include: i => i.Include(x => x.Employee));
		}
		public async Task<IEnumerable<EmployeeActiveAddressModel>> EmployeePresentAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type)
		{
			return await _UOW.GetRepositoryAsync<EmployeePresentAd>().GetWithSelectAsync(
			   selector: d => new EmployeeActiveAddressModel
			   {
				   AddressLineOne = d.AddressLineOne,
				   AddressLineTwo = d.AddressLineTwo,
				   CityOrTown = d.CityOrTown,
				   Country = d.Country,
				   State = d.State,
				   EmpName = d.Employee.Name
			   },
				 predicate: x => x.AddressLineOne != "" && (((!joinDateFrom.HasValue || x.Employee.DateOfJoining >= joinDateFrom)
								&& (!joinDateTo.HasValue || x.Employee.DateOfJoining <= joinDateTo))
								&& ((departmentId == null || x.Employee.DepartmentId == departmentId)
								&& (designationId == null || x.Employee.DesignationId == designationId)
								&& (employeeId == null || x.EmployeeId == employeeId)
								&& (teamId == null || x.Employee.TeamId == teamId))),
						   include: i => i.Include(x => x.Employee));
		}
		public async Task<IEnumerable<EmployeeActiveAddressModel>> EmployeePermanentAddress(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? joinDateFrom, DateTime? joinDateTo, int? type)
		{
			return await _UOW.GetRepositoryAsync<EmployeePermanentAd>().GetWithSelectAsync(
			   selector: d => new EmployeeActiveAddressModel
			   {
				   AddressLineOne = d.AddressLineOne,
				   AddressLineTwo = d.AddressLineTwo,
				   CityOrTown = d.CityOrTown,
				   Country = d.Country,
				   State = d.State,
				   EmpName = d.Employee.Name
			   },
				predicate: x => x.AddressLineOne != "" && (((!joinDateFrom.HasValue || x.Employee.DateOfJoining >= joinDateFrom)
								&& (!joinDateTo.HasValue || x.Employee.DateOfJoining <= joinDateTo))
								&& ((departmentId == null || x.Employee.DepartmentId == departmentId)
								&& (designationId == null || x.Employee.DesignationId == designationId)
								&& (employeeId == null || x.EmployeeId == employeeId)
								&& (teamId == null || x.Employee.TeamId == teamId))),
							   include: i => i.Include(x => x.Employee));
		}
		public async Task<IEnumerable<AllActiveEmployeesReportModel>> AllActiveEmployees(ActiveEmployeeRptModel model)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				 selector: d => new AllActiveEmployeesReportModel
				 {
					 EmpCode = d.No,
					 Name = d.Name,
					 PanNumber = d.PanNumber,
					 MobileNumber = d.MobileNumber,
					 Department = d.Department.Name,
					 Designation = d.Designation.Name,
					 DateOfJoining = d.DateOfJoining,
					 PersonalEmail = d.PersonalEmail,
					 WorkEmail = d.WorkEmail,
					 Team = d.Team.Name,
					 WorkLocation = d.WorkLocation.Name,
					 DateOfBirth = d.DateOfBirth,
					 Gender = d.Gender,
					 FatherName = d.FatherName,
					 AadhaarNumber = d.AadhaarNumber,
					 ReportingTo = d.ReportingToId != null ? d.ReportingTo.Name : "",
					 WorkType = d.WorkType.Name,
					 EmpCategory = d.EmpCategoryId != null ? d.EmpCategory.Name : "",
					 ProfileStatus = d.ProfileStatus,
					 MaritalStatus = d.MaritalStatus,
					 BloodGroup = d.BloodGroup,
					 Status = d.Status
				 },
				predicate: x => x.LastWorkingDate == null
								&& (!model.FromDate.HasValue || x.DateOfJoining >= model.FromDate)
								&& (!model.ToDate.HasValue || x.DateOfJoining <= model.ToDate)
								&& (string.IsNullOrEmpty(model.DateOfBirth.ToString()) || x.DateOfBirth == model.DateOfBirth)
								&& (model.DepartmentId == null || x.DepartmentId == model.DepartmentId)
								&& (model.DesignationId == null || x.DesignationId == model.DesignationId)
								&& (model.EmployeeId == null || x.ID == model.EmployeeId)
								&& (model.TeamId == null || x.TeamId == model.TeamId)
								&& (model.WorkTypeId == null || x.WorkTypeId == model.WorkTypeId)
								&& (model.EmpCategoryId == null || x.EmpCategoryId == model.EmpCategoryId)
								,
			   include: i => i.Include(x => x.WorkType).Include(x => x.Team).Include(x => x.EmpCategory)
							.Include(x => x.ReportingTo).Include(x => x.Department)
							.Include(x => x.Designation).Include(x => x.WorkLocation));
		}
		public async Task<IEnumerable<EmployeeContactReportModel>> AllEmployeeContactDetails(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? fromDate, DateTime? toDate)
		{
			return await _UOW.GetRepositoryAsync<EmployeeFamily>().GetWithSelectAsync(
				   selector: d => new EmployeeContactReportModel
				   {
					   ID = d.ID,
					   PersonName = d.PersonName,
					   HumanRelation = d.HumanRelation,
					   ContactNo = d.ContactNo,
					   Department = d.Employee.Department.Name,
					   DateOfJoining = d.Employee.DateOfJoining,
					   Designation = d.Employee.Designation.Name,
					   No = d.Employee.No,
					   Name = d.Employee.Name
				   },
				   predicate: x => (((!fromDate.HasValue || x.Employee.DateOfJoining >= fromDate)
								&& (!toDate.HasValue || x.Employee.DateOfJoining <= toDate))

									 && ((departmentId == null || x.Employee.DepartmentId == departmentId)
									 && (designationId == null || x.Employee.DesignationId == designationId)
									 && (employeeId == null || x.EmployeeId == employeeId)
									 && (teamId == null || x.Employee.TeamId == teamId))),
					 include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
									.Include(x => x.Employee).ThenInclude(x => x.Designation));
		}
		public async Task<IEnumerable<LeaveBalancesModel>> MyTeamLeaveBalances(Guid? designationId, Guid? employeeId, Guid? leaveTypeId, Guid reportingToId)
		{
			return await _repository.MyTeamLeaveBalanceReport(designationId, employeeId, leaveTypeId, reportingToId);
		}
		public async Task<IEnumerable<LeaveBalancesModel>> LeaveBalances(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? leaveTypeId)
		{
			return await _repository.BalanceReport(departmentId, designationId, teamId, null, leaveTypeId);
		}
		public async Task<IPaginate<LeaveBalanceDetailsModel>> LeaveBalanceDetails(BaseSearch search)
		{
			return await _repository.LeaveBalanceDetails(search);
		}
		public async Task<IEnumerable<ReportingEmployeeModel>> ReportingToEmployee(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, Guid? reportingToId)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				selector: x => new ReportingEmployeeModel
				{
					EmployeeCode = x.No,
					EmployeeName = x.Name,
					Department = x.Department.Name,
					Designation = x.Designation.Name,
					ReportingTo = x.ReportingTo.Name,
				},
				predicate: p => (!departmentId.HasValue || p.DepartmentId == departmentId)
								&& (!designationId.HasValue || p.DesignationId == designationId)
								&& (!teamId.HasValue || p.TeamId == teamId)
								&& (!employeeId.HasValue || p.ID == employeeId)
								&& (!reportingToId.HasValue || p.ReportingToId == reportingToId)
								&& p.ReportingToId.HasValue
				);
		}

		public async Task<IEnumerable<LOBModel>> EmployeeLOBUnique(Guid departmentId)
		{
			var list = await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				selector: x => new
				{
					ID = x.LOBId.Value,
					Name = x.LOB.Name,
				},
				predicate: p => p.LOB.Status == true && p.DepartmentId == departmentId,
				orderBy: x => x.OrderBy(x => x.Name));

			return list.Distinct().OrderBy(x => x.Name).Select(x => new LOBModel { ID = x.ID, Name = x.Name, Status = true });
		}

		public async Task<IEnumerable<FunctionalAreaModel>> FunctionalAreaUnique(Guid departmentId, Guid? lobId)
		{

			var list = await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				selector: x => new
				{
					ID = x.FunctionalAreaId.Value,
					Name = x.FunctionalArea.Name,
				},
				predicate: p => p.FunctionalArea.Status == true && p.DepartmentId == departmentId &&
					p.FunctionalAreaId != null && (!lobId.HasValue || p.LOBId == lobId),
				orderBy: x => x.OrderBy(x => x.Name)
				);

			return list.Distinct().OrderBy(x => x.Name).Select(x => new FunctionalAreaModel { ID = x.ID, Name = x.Name, Status = true });
		}

		public async Task<IEnumerable<DesignationModel>> DesignationsUnique(Guid departmentId, Guid? lobId, Guid? functionalAreaId)
		{
			var list = await _UOW.GetRepositoryAsync<Employee>().GetWithSelectAsync(
				selector: x => new
				{
					ID = x.DesignationId,
					Name = x.Designation.Name,
				},
				predicate: p => p.Designation.Status == true && p.DepartmentId == departmentId &&
				(!lobId.HasValue || p.LOBId == lobId) &&
				  (!functionalAreaId.HasValue || p.FunctionalAreaId == functionalAreaId),
				orderBy: x => x.OrderBy(x => x.Name)
				);

			return list.Distinct().OrderBy(x => x.Name).Select(x => new DesignationModel { ID = x.ID, Name = x.Name, Status = true });
		}
		public async Task<IEnumerable<TranSmart.Domain.Entities.Page>> PagesUnique(Guid moduleId)
		{
			var list = await _UOW.GetRepositoryAsync<TranSmart.Domain.Entities.Page>().GetWithSelectAsync(
				selector: x => new
				{
					x.ID,
					x.Name,
				},
				predicate: p => p.GroupId == moduleId,
				orderBy: x => x.OrderBy(x => x.Name)
				);

			return list.Distinct().OrderBy(x => x.Name).Select(x => new Domain.Entities.Page { ID = x.ID, Name = x.Name });
		}
		public async Task<IEnumerable<UserReportModel>> PageEmployees(Guid? pageId)
		{
			return await _roleRepository.PageEmployees(pageId);
		}
		public async Task<IEnumerable<RoleReportModel>> PageRole(Guid? moduleId, Guid? pageId)
		{
			return await _UOW.GetRepositoryAsync<RolePrivilege>().GetWithSelectAsync(
			   selector: d => new RoleReportModel
			   {
				   RoleName = d.Role.Name,
				   View = (d.Privilege & (int)Privilege.View) == (int)Privilege.View ? "Yes" : "No",
				   Add = (d.Privilege & (int)Privilege.Add) == (int)Privilege.Add ? "Yes" : "No",
				   Update = (d.Privilege & (int)Privilege.Update) == (int)Privilege.Update ? "Yes" : "No",
				   Delete = (d.Privilege & (int)Privilege.Delete) == (int)Privilege.Delete ? "Yes" : "No",
			   },
			   predicate: x => x.PageId == pageId && x.Privilege != 0 && x.Page.GroupId == moduleId,
				include: i => i.Include(x => x.Role),
			   orderBy: x => x.OrderBy(x => x.Role.Name));
		}
	}
}
