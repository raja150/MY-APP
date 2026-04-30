using TranSmart.API.Services;

namespace TranSmart.API.ManageServices
{
	public class DepartmentGroupManageService : IManageService
	{
		private readonly ICacheService _cacheService;
		public DepartmentGroupManageService(ICacheService cacheService)
		{
			_cacheService = cacheService;
		}

		public async Task PostAction(Guid id)
		{
			await _cacheService.UpdateDeptTemp(id);
		}

		public async Task PutAction(Guid id)
		{
			await _cacheService.UpdateDeptTemp(id);
		}
	}
}
