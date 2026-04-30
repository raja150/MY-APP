using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TranSmart.Core;
using Xunit;

namespace Transmart.Services.UnitTests.Core
{
	public class ApplicationUserTest
	{
		private readonly HttpContextAccessor contextAccessor;
		private readonly ClaimsPrincipal claimsPrincipal;
		public ApplicationUserTest()
		{
			var context = new DefaultHttpContext();
			contextAccessor = new HttpContextAccessor();

			var claims = new[]{
						new Claim("uid","User"),
						new Claim("id", "234523523"),
						new Claim("eid", "f7a444df-4f73-4965-b70e-13ce73ecf36f"),
						new Claim(ClaimTypes.Role,  "role" ),
					};

			var identity = new ClaimsIdentity(claims, "TestAuthType");
			claimsPrincipal = new ClaimsPrincipal(identity);

			contextAccessor.HttpContext = context;
			context.User = claimsPrincipal;
		}

		[Fact]
		public void GetUserName_UserName()
		{
			var ss = new ApplicationUser(contextAccessor);
			var response = ss.GetUserName();
			Assert.Equal("User", response);
		}
	}
}
