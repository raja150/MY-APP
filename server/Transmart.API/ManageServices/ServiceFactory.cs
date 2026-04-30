using System;

namespace TranSmart.API.ManageServices
{
	public interface IServiceFactory
	{
		IManageService GetService(object context);
	}
	public class ServiceFactory : IServiceFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public ServiceFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		private Type GetSpecificServiceType(object context)
		{
			// For the sake of simplicity, let's say the context is actually a string with the service type name
			return Type.GetType($"TranSmart.API.ManageServices.{context}ManageService", false, true);
		}

		public IManageService GetService(object context)
		{
			var service = (IManageService)_serviceProvider.GetService(GetSpecificServiceType(context));
			return service ?? (IManageService)_serviceProvider.GetService(typeof(IManageService));
		}
	}
}
