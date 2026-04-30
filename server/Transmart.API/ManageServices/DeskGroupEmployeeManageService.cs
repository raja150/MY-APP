using TranSmart.API.Services;

namespace TranSmart.API.ManageServices
{
	public class DeskGroupEmployeeManageService : IManageService
	{
		private readonly ICacheService _cacheService;
		public DeskGroupEmployeeManageService(ICacheService cacheService)
		{	
			_cacheService = cacheService;
		}
		public async Task PostAction(Guid id)
		{
			await _cacheService.UpdateTempData(id);
		}

		public async Task PutAction(Guid id)
		{
			await _cacheService.UpdateTempData(id);
		}
	}
}
