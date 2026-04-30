using System;
using System.Threading.Tasks;

namespace TranSmart.API.ManageServices
{
	public interface IManageService
	{
		Task PostAction(Guid id);

		Task PutAction(Guid id);
	}

	public class ManageService : IManageService
	{
		public Task PostAction(Guid id)
		{
			return Task.CompletedTask;
		}

		public Task PutAction(Guid id)
		{
			return Task.CompletedTask;
		}
	}
}
