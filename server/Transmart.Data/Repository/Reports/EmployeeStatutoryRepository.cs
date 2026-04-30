using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Reports;

namespace TranSmart.Data.Repository.Reports
{
	public interface IEmployeeStatutoryRepository
	{
		Task<IEnumerable<EmployeeEpfModel>> GetProvidentFundInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type);
		Task<IEnumerable<EmployeeEsiModel>> GetESIInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type);
	}
	public class EmployeeStatutoryRepository : IEmployeeStatutoryRepository
	{
		private readonly TranSmartContext _context;
		public EmployeeStatutoryRepository(TranSmartContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<EmployeeEpfModel>> GetProvidentFundInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type)
		{
			return await (from emp in _context.Organization_Employee
						  join es in _context.Payroll_EmpStatutory on emp.ID equals es.EmpId into g
						  from es in g.DefaultIfEmpty()
						  where !emp.LastWorkingDate.HasValue
												&& ((departmentId == null || emp.DepartmentId == departmentId)
												&& (designationId == null || emp.DesignationId == designationId)
												&& (employeeId == null || emp.ID == employeeId)
												&& (teamId == null || emp.TeamId == teamId))
												&& ((type == null || type == 1)
												|| (type == 2 && es != null && es.EnablePF == 1)
												|| (type == 3 && es != null && es.EnablePF == 0)
												|| (type == 4 && es == null))
						  select new EmployeeEpfModel
						  {
							  UANNo = es.UAN,
							  EmployeeName = emp.Name,
							  EmployeeCode = emp.No,
							  Designation = emp.Designation.Name,
							  DateOfJoining = emp.DateOfJoining,
							  Department = emp.Department.Name,
							  EmployeePFNo = es.EmployeesProvid,
							  EmployeeContrib = es.EmployeeContrib == null ? 0 : es.EmployeeContrib,
							  EPS = es == null ? "" : es.EPS ? "Yes" : "No",
							  EmployeePF = es.EnablePF == 1 ? "Yes" : es.EnablePF == 0 ? "No" : "N/A",
						  }).OrderBy(x => x.EmployeeName).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeEsiModel>> GetESIInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type)
		{
			return await (from emp in _context.Organization_Employee
						  join es in _context.Payroll_EmpStatutory on emp.ID equals es.EmpId into g
						  from es in g.DefaultIfEmpty()
						  where !emp.LastWorkingDate.HasValue
												&& ((departmentId == null || emp.DepartmentId == departmentId)
												&& (designationId == null || emp.DesignationId == designationId)
												&& (employeeId == null || emp.ID == employeeId)
												&& (teamId == null || emp.TeamId == teamId))
												&& ((type == null || type == 1)
												|| (type == 2 && es != null && es.EnableESI == 1)
												|| (type == 3 && es != null && es.EnableESI == 0)
												|| (type == 4 && es == null))
						  select new EmployeeEsiModel
						  {
							  UANNo = es.UAN,
							  EmployeeName = emp.Name,
							  EmployeeCode = emp.No,
							  Designation = emp.Designation.Name,
							  DateOfJoining = emp.DateOfJoining,
							  Department = emp.Department.Name,
							  EPS = es == null ? "" : es.EPS ? "Yes" : "No",
							  ESINo = es.ESINo,
							  EmployeeESI = es.EnableESI == 1 ? "Yes" : es.EnableESI == 0 ? "No" : "N/A",
						  }).OrderBy(x => x.EmployeeName).ToListAsync();
		}
	}
}
