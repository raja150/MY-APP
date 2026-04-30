using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
	public partial interface IEmployeeService : IBaseService<Employee>
	{
		Task<Employee> GetEmp(Guid EmpId);
		Task<IEnumerable<Employee>> SearchEmp(string name, Guid loginId, Guid raiseById);
		Task<IEnumerable<Employee>> SearchEmp(string name);
		Task<IEnumerable<Employee>> GetEmps(DateTime startDate);
		Task<IEnumerable<Employee>> GetBirthdays(Guid? departmentId);
		Task<IEnumerable<Employee>> GetBirthdays(DateTime fromDate, DateTime toDate);
		Task<Employee> GetDetails(Guid EmpId);
		Task<EmployeeResignation> GetResignationDetails(Guid EmpId);
		Task<IEnumerable<EmployeeFamily>> GetContactDetails(Guid EmpId);
		Task<EmployeePresentAd> GetPresentAddress(Guid EmpId);
		Task<EmployeePermanentAd> GetPermanentAddress(Guid EmpId);
		Task<EmployeeEmergencyAd> GetEmergencyAddress(Guid EmpId);
		Task<IEnumerable<ApplyLeave>> LeavesApprovedEmployees(Guid? departmentId);
		Task<Employee> GetLoginEmpMail(Guid id);
		Task<IEnumerable<ApplyLeave>> ApprovedPendingEmployees(Guid employeeId);
		Task<IEnumerable<ApplyWfh>> ApprovedPendingWFHEmployees(Guid employeeId);
		Task<Result<Employee>> AddEmployee(List<Employee> items);

	}
	public partial class EmployeeService : BaseService<Employee>, IEmployeeService
	{
		public async Task<Employee> GetEmp(Guid EmpId)
		{
			return await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == EmpId,
				include: i => i.Include(x => x.ReportingTo),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public async Task<Employee> GetDetails(Guid EmpId)
		{
			return await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == EmpId,
				include: i => i.Include(x => x.ReportingTo).Include(x => x.Department)
				.Include(x => x.Designation).Include(x => x.WorkLocation).Include(x => x.WorkType)
				.Include(x => x.Team).Include(x => x.ReportingTo),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public async Task<EmployeeResignation> GetResignationDetails(Guid EmpId)
		{
			return await UOW.GetRepositoryAsync<EmployeeResignation>().SingleAsync(x => x.EmployeeId == EmpId, include: i => i.Include(x => x.Employee),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public Task<IEnumerable<EmployeeFamily>> GetContactDetails(Guid EmpId)
		{
			return UOW.GetRepositoryAsync<EmployeeFamily>().GetAsync(x => x.EmployeeId == EmpId,
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public Task<EmployeePresentAd> GetPresentAddress(Guid EmpId)
		{
			return UOW.GetRepositoryAsync<EmployeePresentAd>().SingleAsync(x => x.EmployeeId == EmpId,
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public Task<EmployeePermanentAd> GetPermanentAddress(Guid EmpId)
		{
			return UOW.GetRepositoryAsync<EmployeePermanentAd>().SingleAsync(x => x.EmployeeId == EmpId,
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public Task<EmployeeEmergencyAd> GetEmergencyAddress(Guid EmpId)
		{
			return UOW.GetRepositoryAsync<EmployeeEmergencyAd>().SingleAsync(x => x.EmployeeId == EmpId,
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public Task<IEnumerable<Employee>> SearchEmp(string name, Guid loginId, Guid raiseById)
		{
			return UOW.GetRepositoryAsync<Employee>().GetAsync(
				predicate: p => p.Name.Contains(name) && p.ID != loginId && p.ID != raiseById,
				include: i => i.Include(x => x.Department),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public Task<IEnumerable<Employee>> SearchEmp(string name)
		{
			return UOW.GetRepositoryAsync<Employee>().GetAsync(
				predicate: p => string.IsNullOrEmpty(name) ||
				p.No.Contains(name) || p.Name.Contains(name) || p.MobileNumber.Contains(name),
				include: i => i.Include(x => x.Department),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public override async Task CustomValidation(Employee item, Result<Employee> result)
		{
			if (item.PanNumber != "")
			{
				if (!Regex.IsMatch(item.PanNumber, Resource.Regx_Pan))
				{
					result.AddMessageItem(new MessageItem
						 (nameof(Employee.PanNumber), Resource.Invalid_PAN_Format));
				}
				else if (await UOW.GetRepositoryAsync<Employee>().HasRecordsAsync(x => x.PanNumber == item.PanNumber && x.ID != item.ID))
				{
					result.AddMessageItem(new MessageItem
						  (nameof(Employee.PanNumber), Resource.Pan_Number_Already_Exists));
				}
			}


			if (item.MobileNumber != "" && !Regex.IsMatch(item.MobileNumber, Resource.Regx_Mobile_Number))
			{
				result.AddMessageItem(new MessageItem
					 (nameof(Employee.MobileNumber), Resource.Lead_ContactNo_Is_Digits_Only));
			}
			else
			{
				if (await UOW.GetRepositoryAsync<Employee>().HasRecordsAsync(x => x.MobileNumber == item.MobileNumber && x.ID != item.ID))
				{
					result.AddMessageItem(new MessageItem
						  (nameof(Employee.MobileNumber), Resource.Lead_ContactNo_Is_Already_Exist));
				}
			}

			if (item.AadhaarNumber != "" && await UOW.GetRepositoryAsync<Employee>().HasRecordsAsync(x => x.AadhaarNumber == item.AadhaarNumber && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem
					  (nameof(Employee.AadhaarNumber), Resource.Aadhar_Already_Exists));
			}

			if (item.WorkEmail != "" && !Regex.IsMatch(item.WorkEmail, Resource.Regx_Email_Validation))
			{
				result.AddMessageItem(new MessageItem
					 (nameof(Employee.WorkEmail), Resource.Invalid_Email_Format));
			}
			if (item.PersonalEmail != "" && !Regex.IsMatch(item.PersonalEmail, Resource.Regx_Email_Validation))
			{
				result.AddMessageItem(new MessageItem
					 (nameof(Employee.PersonalEmail), Resource.Invalid_Email_Format));
			}
			if (item.PassportNumber != "" && !Regex.IsMatch(item.PassportNumber, Resource.Regx_Passport))
			{
				result.AddMessageItem(new MessageItem
				   (nameof(Employee.PassportNumber), Resource.Invalid_Format));
			}
			if (item.DateOfJoining.Year - item.DateOfBirth.Year < 15)
			{
				result.AddMessageItem(new MessageItem
				   (nameof(Employee.DateOfJoining), Resource.Age_should_be_greater_than_15));
			}
			var user = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.EmployeeID == item.ID);
			if (user != null)
			{
				user.Type = item.Status == 1 ? (int)UserSts.Active : (int)UserSts.InActive;
				user.Name = item.No.Trim();
				UOW.GetRepositoryAsync<User>().UpdateAsync(user);
			}
		}
		public override async Task OnAfterAdd(Employee item, Result<Employee> executionResult)
		{
			if (executionResult.IsSuccess)
			{
				await UOW.GetRepositoryAsync<Replication>().AddAsync(new Replication
				{
					DepartmentId = item.DepartmentId,
					Type = (byte)ReplicationType.Add,
					Category = (byte)ReplicationCategory.Empployee,
					RefId = item.ID,
				});
				await UOW.SaveChangesAsync();
			}
			await base.OnAfterAdd(item, executionResult);
		}
		public override async Task OnAfterUpdate(Employee item, Result<Employee> executionResult)
		{
			if (executionResult.IsSuccess)
			{
				var replication = await UOW.GetRepositoryAsync<Replication>().SingleAsync(x => x.RefId == item.ID
							&& x.Category == (byte)ReplicationCategory.Empployee
							&& x.Type == (byte)ReplicationType.Update);

				if (replication == null)
				{
					await UOW.GetRepositoryAsync<Replication>().AddAsync(new Replication
					{
						DepartmentId = item.DepartmentId,
						Type = (byte)ReplicationType.Update,
						Category = (byte)ReplicationCategory.Empployee,
						RefId = item.ID,
					});
				}
				else
				{
					replication.Status = false;
					UOW.GetRepositoryAsync<Replication>().UpdateAsync(replication);
				}
				await UOW.SaveChangesAsync();
			}
			await base.OnAfterUpdate(item, executionResult);
		}

		public async Task<IEnumerable<Employee>> GetEmps(DateTime startDate)
		{
			return await UOW.GetRepositoryAsync<Employee>().GetAsync(x =>
								 (x.LastWorkingDate == null && x.Status == 1) || x.LastWorkingDate >= startDate,
							 include: x => x.Include(x => x.Designation).Include(x => x.Department));
		}
		public async Task<IEnumerable<Employee>> GetBirthdays(Guid? departmentId)
		{
			return await UOW.GetRepositoryAsync<Employee>().GetAsync(
				x => x.DateOfBirth.AddYears(DateTime.Now.Year - x.DateOfBirth.Year) >= DateTime.Now.Date
				&& x.DateOfBirth.AddYears(DateTime.Now.Year - x.DateOfBirth.Year) <= TimeStamp().AddDays(5)
				&& (!departmentId.HasValue || x.DepartmentId == departmentId) && x.Status == (int)EmployeeStatus.Active && !x.LastWorkingDate.HasValue,
				orderBy: x => x.OrderBy(i => i.DateOfBirth.Month).ThenBy(i => i.DateOfBirth.Day));
		}
		public async Task<IEnumerable<Employee>> GetBirthdays(DateTime fromDate, DateTime toDate)
		{
			return await UOW.GetRepositoryAsync<Employee>().GetAsync(
				x => x.DateOfBirth.AddYears(DateTime.Now.Year - x.DateOfBirth.Year) >= fromDate.Date
				&& x.DateOfBirth.AddYears(DateTime.Now.Year - x.DateOfBirth.Year) <= toDate.Date
				&& x.Status == (int)EmployeeStatus.Active && !x.LastWorkingDate.HasValue,
				include: x=>x.Include(o=>o.Department).Include(o => o.Designation),
				orderBy: x => x.OrderBy(i => i.DateOfBirth.Month).ThenBy(i => i.DateOfBirth.Day));
		}
		public async Task<IEnumerable<ApplyLeave>> LeavesApprovedEmployees(Guid? departmentId)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync(
				predicate: p => p.Status == (int)Domain.Enums.ApplyLeaveSts.Approved
						   && (!departmentId.HasValue || p.Employee.DepartmentId == departmentId)
						&& p.FromDate.Date <= DateTime.Today && p.ToDate.Date >= DateTime.Today,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Team));
		}
		public async Task<IEnumerable<ApplyLeave>> ApprovedPendingEmployees(Guid employeeId)
		{
			return await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync(predicate: p => p.Status == (int)ApplyLeaveSts.Applied
																				   && (p.Employee.ReportingToId == employeeId 
																				   || p.Employee.ReportingTo.ReportingToId == employeeId),
				include: i => i.Include(x => x.Employee));
		}
		public async Task<IEnumerable<ApplyWfh>> ApprovedPendingWFHEmployees(Guid employeeId)
		{
			return await UOW.GetRepositoryAsync<ApplyWfh>().GetAsync(predicate: p => p.Status == (int)Domain.Enums.WfhStatus.Applied
																				   && p.Employee.ReportingToId == employeeId,
				include: i => i.Include(x => x.Employee));
		}

		public async Task<Employee> GetLoginEmpMail(Guid id)
		{
			return await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == id);
		}

		public async Task<Result<Employee>> AddEmployee(List<Employee> items)
		{
			foreach (var emp in items)
			{
				Employee empInfo = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.No == emp.No);
				if (empInfo == null)
				{
					await UOW.GetRepositoryAsync<Employee>().AddAsync(emp);
					await UOW.GetRepositoryAsync<Replication>().AddAsync(new Replication
					{
						DepartmentId = emp.DepartmentId,
						Type = (byte)ReplicationType.Add,
						Category = (byte)ReplicationCategory.Empployee,
						RefId = emp.ID,
					});
				}
			}
			Result<Employee> result = new();
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

	}
}
