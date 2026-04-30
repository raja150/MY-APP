using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service
{
	public partial interface ISearchService
	{
		Task<IEnumerable<RolePrivilege>> GetRolePrivilige();
		Task<IEnumerable<RoleReportPrivilege>> GetRoleReportPrivilige();
		Task<IEnumerable<PayMonth>> GetPayMonths(string name);
		Task<IEnumerable<Employee>> GetEmployee(string name);
		Task<IEnumerable<Employee>> GetEmployeeDetails(string name);
		Task<IEnumerable<Employee>> GetEmployeeBySearch();
		Task<IEnumerable<DesignationModel>> GetDesignations(Guid monthId);
		Task<Result<IEnumerable<Employee>>> GetTeamEmployeeBySearch(Guid loginUserEmpId);
		Task<Employee> GetEmpById(Guid empId);
		Task<IEnumerable<Token>> GetApiKeyUsers();
		Task<User> GetUserById(Guid empId);

		Task<IEnumerable<Result>> GetResultPapers();
		Task<IEnumerable<Result>> GetResultEmployees();
		Task<IEnumerable<Paper>> GetPapers();

	}

	public partial class SearchService : ISearchService
	{
		private readonly IUnitOfWork _UOW;
		public SearchService(IUnitOfWork uow)
		{
			_UOW = uow;
		}
		public async Task<IEnumerable<Token>> GetApiKeyUsers()
		{
			return await _UOW.GetRepositoryAsync<Token>().GetAsync(include: x => x.Include(x => x.User));
		}

		public async Task<IEnumerable<RolePrivilege>> GetRolePrivilige()
		{
			return await _UOW.GetRepositoryAsync<RolePrivilege>().GetAsync(
				  //predicate: x => x.Privilege > 0,
				  include: x => x.Include(i => i.Page).ThenInclude(o => o.Group),
				  orderBy: o => o.OrderBy(d => d.Page.Group.DisplayOrder).ThenBy(dd => dd.Page.DisplayOrder));
		}

		public async Task<IEnumerable<RoleReportPrivilege>> GetRoleReportPrivilige()
		{
			return await _UOW.GetRepositoryAsync<RoleReportPrivilege>().GetAsync(
				  //predicate: x => x.CanView,
				  include: x => x.Include(i => i.Report).ThenInclude(o => o.Module),
				  orderBy: o => o.OrderBy(d => d.Report.Module.Name).ThenBy(dd => dd.Report.Name));
		}
		public async Task<IEnumerable<PayMonth>> GetPayMonths(string name)
		{
			return await _UOW.GetRepositoryAsync<PayMonth>().GetAsync(x => x.Name.Contains(name));
		}
		public async Task<IEnumerable<Employee>> GetEmployee(string name)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetAsync(x => x.Name.Contains(name));
		}
		public async Task<IEnumerable<Employee>> GetEmployeeDetails(string name)
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetAsync(
				predicate: x => x.Name.Contains(name) || x.MobileNumber.Contains(name),
				include: i => i.Include(x => x.Department).Include(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public async Task<IEnumerable<Employee>> GetEmployeeBySearch()
		{
			return await _UOW.GetRepositoryAsync<Employee>().GetAsync(
				include: i => i.Include(x => x.Department).Include(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public async Task<Result<IEnumerable<Employee>>> GetTeamEmployeeBySearch(Guid loginUserEmpId)
		{
			Result<IEnumerable<Employee>> result = new();
			Employee loginUser = await _UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == loginUserEmpId,
				include: i => i.Include(x => x.Team));
			if (loginUser.TeamId == null)
			{
				return null;
			}

			var data = await _UOW.GetRepositoryAsync<Employee>().GetAsync(
				predicate: p => p.TeamId == loginUser.TeamId,
				include: i => i.Include(x => x.Department).Include(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
			result.ReturnValue = data;
			return result;
		}

		public async Task<IEnumerable<DesignationModel>> GetDesignations(Guid monthId)
		{
			var model = new List<DesignationModel>();
			var paySheet = await _UOW.GetRepositoryAsync<PaySheet>().GetAsync(
				predicate: p => p.PayMonthId == monthId,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation));
			foreach (var item in paySheet)
			{
				var dis = await _UOW.GetRepositoryAsync<Designation>().GetAsync(x => x.ID == item.Employee.DesignationId && x.Status);
				foreach (var disgn in dis)
				{
					model.Add(new DesignationModel
					{
						Name = disgn.Name,
						ID = disgn.ID
					});
				}
			}
			return model;
		}

		public async Task<Employee> GetEmpById(Guid empId)
		{
			return await _UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == empId);
		}

		public async Task<User> GetUserById(Guid empId)
		{
			return await _UOW.GetRepositoryAsync<User>().SingleAsync(x => x.EmployeeID == empId);
		}


		//Result
		public async Task<IEnumerable<Result>> GetResultPapers()
		{
			return await _UOW.GetRepositoryAsync<Result>().GetAsync(include: x => x.Include(x => x.Paper));
		}

		public async Task<IEnumerable<Result>> GetResultEmployees()
		{
			return (await _UOW.GetRepositoryAsync<Result>().GetAsync(include: x => x.Include(x => x.Employee))).DistinctBy(x => x.EmployeeId);
		}

		//Paper
		public async Task<IEnumerable<Paper>> GetPapers()
		{
			return await _UOW.GetRepositoryAsync<Paper>().GetAsync();
		}
	}
}
