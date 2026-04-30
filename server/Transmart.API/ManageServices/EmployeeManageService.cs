using System;
using System.Threading.Tasks;
using TranSmart.API.Services;

namespace TranSmart.API.ManageServices
{
	public class EmployeeManageService : IManageService
	{
		private readonly ICacheService _cacheService;
		public EmployeeManageService(ICacheService cacheService)
		{
			_cacheService = cacheService;
		}

		public async Task PostAction(Guid id)
		{
			await _cacheService.UpdateEmployees(id);
		}

		public async Task PutAction(Guid id)
		{
			await _cacheService.UpdateEmployees(id);
		}
	}
}
