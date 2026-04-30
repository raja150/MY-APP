using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.AppSettings;

namespace TranSmart.Service.AppSettings
{
	public partial class UserSettingsService
	{
		public override async Task OnBeforeAdd(UserSettings item, Result<UserSettings> executionResult)
		{
			var records = await UOW.GetRepositoryAsync<UserSettings>().GetCountAsync();
			if (records > 0)
			{
				executionResult.AddMessageItem(new MessageItem(Resource.Only_One_Record_Is_Accepted));
			}
			await base.OnBeforeAdd(item, executionResult);
		}
	}
}
