using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Reports;

namespace TranSmart.Data.Repository.Reports
{
	public interface IRoleRepository
	{
		Task<IEnumerable<UserReportModel>> PageEmployees(Guid? pageId);
	}
	public class RoleRepository : IRoleRepository
	{
		private readonly TranSmartContext _context;
		public RoleRepository(TranSmartContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<UserReportModel>> PageEmployees(Guid? pageId)
		{
			return await (from rolePrivilege in _context.RolePrivileges
						  join role in _context.Roles on rolePrivilege.RoleId equals role.ID
						  join user in _context.Users on rolePrivilege.RoleId equals user.RoleId
						  join emp in _context.Organization_Employee on user.EmployeeID equals emp.ID
						  join dep in _context.Organization_Department on emp.DepartmentId equals dep.ID
						  join deg in _context.Organization_Designation on emp.DesignationId equals deg.ID
						  where rolePrivilege.PageId == pageId && rolePrivilege.Privilege != 0
						  select new UserReportModel
						  {
							  EmployeeName = emp.Name,
							  EmployeeCode = emp.No,
							  Department = dep.Name,
							  Designation = deg.Name,
							  RoleName = role.Name,
						  }).OrderBy(x => x.EmployeeName).ToListAsync();
		}
	}
}
