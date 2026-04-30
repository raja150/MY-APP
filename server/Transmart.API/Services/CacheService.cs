using Microsoft.Extensions.Caching.Memory;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Cache;
using TranSmart.Domain.Models.Cache.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.Response;
using TranSmart.Service;
using TranSmart.Service.Helpdesk;

namespace TranSmart.API.Services
{
	public interface ICacheService
	{
		Task<List<RolePrivilegeCache>> GetRolePrivileges();
		Task<List<RolePrivilegeCache>> UpdateRolePrivileges();
		Task<List<RoleReportPrivilegeCache>> GetRoleReportPrivileges();
		Task<List<RoleReportPrivilegeCache>> UpdateRoleReportPrivileges();
		Task<List<RolePrivilegeCache>> GetRoleMenu(Guid roleId);
		Task<List<RoleReportPrivilegeCache>> GetReportsMenu(Guid moduleId, Guid roleId);
		Task<List<ReportModule>> GetRoleReportModule(Guid roleId);
		Task<IEnumerable<EmployeeSearchCache>> SearchedEmployee(string name, Guid? loginUserDepartmentId);
		Task<List<EmployeeSearchCache>> UpdateEmployees(Guid empId);
		Task<IEnumerable<EmployeeSearchCache>> GetTeamEmployeeBySearch(Guid loginUserId, string name);
		bool PayRunUser();
		void RemovePayRunUser();
		Task<ApiKeyCache> GetUserByApiKey(string apikey);
		//Task<List<RoleReportCache>> GetRoleReports(Guid moduleId, Guid roleId);
		Task UpdateDeptTemp(Guid id);
		Task UpdateTempData(Guid deskGroupEmpId);
		Task<List<DeskGroupEmpCache>> DeskGroupEmps(Guid groupId);
		Task<IEnumerable<ResultEmpModel>> GetResultEmp(string name);
		Task<IEnumerable<ResultPaperModel>> GetResultPaper(string name);
		Task<IEnumerable<PaperSearchCache>> GetPaper(string name);


	}

	public class CacheService : ICacheService
	{
		private readonly IMapper _mapper;
		private readonly ISearchService _service;
		private readonly IMemoryCache _memoryCache;
		private readonly ISsoService _ssoService;
		public CacheService(IMapper mapper, IMemoryCache memoryCache, ISearchService service,
			ISsoService ssoService)
		{
			_mapper = mapper;
			_memoryCache = memoryCache;
			_service = service;
			_ssoService = ssoService;
		}
		public async Task<ApiKeyCache> GetUserByApiKey(string apikey)
		{
			string cacheKey = "apiKeys";
			if (_memoryCache.TryGetValue(cacheKey, out List<ApiKeyCache> list))
			{
				if (list != null)
				{
					return list.FirstOrDefault(x => x.Key.Equals(apikey, StringComparison.Ordinal));
				}
			}
			var apiKeys = _mapper.Map<List<ApiKeyCache>>(await _service.GetApiKeyUsers());

			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(10),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};

			_ = _memoryCache.Set(cacheKey, apiKeys, cacheExpiryOptions);
			return apiKeys.FirstOrDefault(x => x.Key.Equals(apikey, StringComparison.Ordinal));
		}

		public async Task<List<RolePrivilegeCache>> GetRolePrivileges()
		{
			string cacheKey = "rolePrivileges";
			return _memoryCache.TryGetValue(cacheKey, out List<RolePrivilegeCache> list) ? list : await UpdateRolePrivileges();
		}
		public async Task<List<RolePrivilegeCache>> GetRoleMenu(Guid roleId)
		{
			string cacheKey = "rolePrivileges";
			if (!_memoryCache.TryGetValue(cacheKey, out List<RolePrivilegeCache> list))
			{
				list = await UpdateRolePrivileges();
			}

			return list.Where(x => x.RoleId == roleId && x.Privilege > 0).ToList();
		}
		public async Task<List<RolePrivilegeCache>> UpdateRolePrivileges()
		{
			var cacheKey = "rolePrivileges";
			List<RolePrivilegeCache> list = _mapper.Map<List<RolePrivilegeCache>>(await _service.GetRolePrivilige());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}

		public async Task<List<RoleReportPrivilegeCache>> GetRoleReportPrivileges()
		{
			var cacheKey = "roleReportPrivileges";
			if (_memoryCache.TryGetValue(cacheKey, out List<RoleReportPrivilegeCache> list))
			{
				return list;
			}
			return await UpdateRoleReportPrivileges();
		}

		public async Task<List<RoleReportPrivilegeCache>> GetRoleReports(Guid moduleId, Guid roleId)
		{
			var cacheKey = "roleReportPrivileges";
			if (!_memoryCache.TryGetValue(cacheKey, out List<RoleReportPrivilegeCache> list))
			{
				list = await UpdateRoleReportPrivileges();
			}
			return list.Where(x => x.RoleId == roleId && x.ModuleId == moduleId).ToList();
		}

		public async Task<List<ReportModule>> GetRoleReportModule(Guid roleId)
		{
			var cacheKey = "roleReportPrivileges";
			if (!_memoryCache.TryGetValue(cacheKey, out List<RoleReportPrivilegeCache> list))
			{
				list = await UpdateRoleReportPrivileges();
			}

			var modules = list.Where(x => x.RoleId == roleId).Select(g => new
			{
				g.ModuleId,
				Name = g.Module,
				Label = g.ModuleLabel
			}).Distinct().ToList();

			var a = modules.Select(x => new ReportModule { ID = x.ModuleId, Label = x.Label, Name = x.Name }).ToList();

			return a;
		}
		public async Task<List<RoleReportPrivilegeCache>> GetReportsMenu(Guid moduleId, Guid roleId)
		{
			var cacheKey = "roleReportPrivileges";
			if (!_memoryCache.TryGetValue(cacheKey, out List<RoleReportPrivilegeCache> list))
			{
				list = await UpdateRoleReportPrivileges();
			}

			return list.Where(x => x.RoleId == roleId && x.ModuleId == moduleId && x.CanView).ToList();
		}
		public async Task<List<RoleReportPrivilegeCache>> UpdateRoleReportPrivileges()
		{
			var cacheKey = "roleReportPrivileges";
			List<RoleReportPrivilegeCache> list = _mapper.Map<List<RoleReportPrivilegeCache>>(await _service.GetRoleReportPrivilige());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}

		public async Task<IEnumerable<EmployeeSearchCache>> SearchedEmployee(string name, Guid? departmentId)
		{
			string cacheKey = "org_Employee";
			if (!_memoryCache.TryGetValue(cacheKey, out List<EmployeeSearchCache> list))
			{
				list = await UpdateEmployees(Guid.Empty);
			}

			return list.Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)
								&& (!departmentId.HasValue || x.DepartmentId == departmentId));
		}
		private async Task UpdateEmpSts(Guid empId)
		{
			//Checking Updated or Created user is exist in User Table or Not
			var user = await _service.GetUserById(empId);
			if (user != null)
			{
				await _ssoService.UpdateEmpSts(new SsoUserModel
				{
					EmployeeId = empId,
					Name = user.Name,
					Type = user.Type,
				});
			}
		}
		public async Task<List<EmployeeSearchCache>> UpdateEmployees(Guid empId)
		{
			if (empId != Guid.Empty)
			{
				//Updating Employee details in SSO
				await UpdateEmpSts(empId);
			}
			List<EmployeeSearchCache> list = _mapper.Map<List<EmployeeSearchCache>>(await _service.GetEmployeeBySearch());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set("org_Employee", list, cacheExpiryOptions);
			return list;
		}
		public async Task<IEnumerable<EmployeeSearchCache>> GetTeamEmployeeBySearch(Guid loginUserId, string name)
		{
			string cacheKey = "org_Employees";
			if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<EmployeeSearchCache> list))
			{
				list = await UpdateTeamEmployees(loginUserId, cacheKey);
				if (list == null)
				{
					return list;
				}
			}

			return list.Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
		public async Task<IEnumerable<EmployeeSearchCache>> UpdateTeamEmployees(Guid loginUserId, string cacheKey = "org_Employees")
		{
			var employee = await _service.GetTeamEmployeeBySearch(loginUserId);
			if (employee == null)
			{
				return null;
			}
			IEnumerable<EmployeeSearchCache> list = _mapper.Map<IEnumerable<EmployeeSearchCache>>(employee.ReturnValue);
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}

		public void RemovePayRunUser()
		{
			var cacheKey = "payRunUserKey";
			if (_memoryCache.TryGetValue(cacheKey, out bool payrollRuning))
			{
				_memoryCache.Remove(cacheKey);
			}
		}
		public bool PayRunUser()
		{
			//Cache value true when payroll is running, otherwise false
			var cacheKey = "payRunUserKey";
			if (_memoryCache.TryGetValue(cacheKey, out bool payrollRuning) && payrollRuning)
			{
				return true;
			}

			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(10),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromMinutes(12)
			};

			_memoryCache.Set(cacheKey, true, cacheExpiryOptions);
			return false;
		}

		public async Task UpdateDeptTemp(Guid id)
		{
			//await _deskService.UpdateTempData();
			await DeskGroupEmps(id);

		}
		public async Task UpdateTempData(Guid deskGroupEmpId)
		{
			//await _deskService.CheckGroupLinkedToDept(deskGroupEmpId);
			await DeskGroupEmps(deskGroupEmpId);
		}
		public async Task<List<DeskGroupEmpCache>> DeskGroupEmps(Guid groupId)
		{
			var cacheKey = "desk_group_emps";
			if (_memoryCache.TryGetValue(cacheKey, out List<DeskGroupEmpCache> emps) && groupId == Guid.Empty)
			{
				if (!emps.Any())
				{
					emps = await UpdateGroupEmps();
				}
				return emps;
			}

			return await UpdateGroupEmps();
		}
		private async Task<List<DeskGroupEmpCache>> UpdateGroupEmps(string cacheKey = "desk_group_emps")
		{
			var list = _mapper.Map<List<DeskGroupEmpCache>>(new object());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}

		#region OnlineTest
		//Result
		public async Task<IEnumerable<ResultEmpModel>> GetResultEmp(string name)
		{
			string cacheKey = "OT_ResultEmp";
			if (!_memoryCache.TryGetValue(cacheKey, out List<ResultEmpModel> list))
			{
				list = await UpdateResultEmployee();
			}
			return list.Where(x => x.EmployeeName.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
		private async Task<List<ResultEmpModel>> UpdateResultEmployee()
		{
			var cacheKey = "OT_ResultEmp";
			List<ResultEmpModel> list = _mapper.Map<List<ResultEmpModel>>(await _service.GetResultEmployees());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}


		public async Task<IEnumerable<ResultPaperModel>> GetResultPaper(string name)
		{
			string cacheKey = "OT_ResultPaper";
			if (!_memoryCache.TryGetValue(cacheKey, out List<ResultPaperModel> list))
			{
				list = await UpdateResultPaper();
			}
			return list.Where(x => x.PaperName.Contains(name, StringComparison.CurrentCultureIgnoreCase)).DistinctBy(x => x.ID).ToList();
		}
		private async Task<List<ResultPaperModel>> UpdateResultPaper()
		{
			var cacheKey = "OT_ResultPaper";
			List<ResultPaperModel> list = _mapper.Map<List<ResultPaperModel>>(await _service.GetResultPapers());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}

		public async Task<IEnumerable<PaperSearchCache>> GetPaper(string name)
		{
			string cacheKey = "OT_Paper_List";
			if (!_memoryCache.TryGetValue(cacheKey, out List<PaperSearchCache> list))
			{
				list = await UpdatePaper();
			}
			return list.Where(x => x.Paper.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
		private async Task<List<PaperSearchCache>> UpdatePaper()
		{
			var cacheKey = "OT_Paper_List";
			List<PaperSearchCache> list = _mapper.Map<List<PaperSearchCache>>(await _service.GetPapers());
			MemoryCacheEntryOptions cacheExpiryOptions = new()
			{
				AbsoluteExpiration = DateTime.Now.AddHours(5),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromHours(2)
			};
			_ = _memoryCache.Set(cacheKey, list, cacheExpiryOptions);
			return list;
		}
		#endregion OnlineTest
	}
}
