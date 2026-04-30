
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TranSmart.Core.Util;

namespace TranSmart.API.Controllers
{
	[ApiController]
	[Authorize(AuthenticationSchemes = StringUtil.APIKey)]
	public class BaseController : ControllerBase
	{
		public bool IS_USER_EMPLOYEE => !(User.Claims.FirstOrDefault(x => x.Type == "eid") == null || string.IsNullOrEmpty(User.Claims.FirstOrDefault(x => x.Type == "eid").Value));
		public Guid LOGIN_USER_EMPId => User.Claims.FirstOrDefault(x => x.Type == "eid") == null || string.IsNullOrEmpty(User.Claims.FirstOrDefault(x => x.Type == "eid").Value) ? Guid.Empty : Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "eid").Value);
		public Guid LOGIN_USER_DEPTID => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "did").Value ?? Guid.Empty.ToString());
		public string UserId => User.Claims.FirstOrDefault(x => x.Type == "uid").Value ?? string.Empty;
		public string IpAddress => HttpContext.Connection.RemoteIpAddress.ToString();
		public Guid RoleId => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value ?? Guid.Empty.ToString());
		public Guid LOGIN_USER_DESGID => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "desgId").Value ?? Guid.Empty.ToString());	

		public override ActionResult ValidationProblem()
		{
			base.ValidationProblem();
			IOptions<ApiBehaviorOptions> options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
			return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);

		}
	}
}
