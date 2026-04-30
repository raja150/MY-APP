using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace TranSmart.Core
{
	public class ApplicationUser : IApplicationUser
	{
		//https://stackoverflow.com/questions/52050501/how-to-access-jwt-user-claims-in-class-library
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ApplicationUser(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string UserId => this.GetUserName();

		public string GetUserName()
		{
			if (_httpContextAccessor.HttpContext != null)
			{
				var data = _httpContextAccessor.HttpContext
								  .User.Claims.FirstOrDefault(x => x.Type == "uid");
				if (data != null)
				{
					return data.Value;
				}
				return "Admin";
			}
			return "Data Seeding";
		}
	}
}
