using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using TranSmart.API.Services;
using TranSmart.Core;

namespace TranSmart.API.Extensions
{
	public class ApiAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		internal readonly Permission _permission;
		internal readonly Privilege _privilege;
		public Permission Permission { get { return _permission; } }
		public Privilege Privilege { get { return _privilege; } }
		public ApiAuthorizeAttribute(Permission permission, Privilege privilege)
		{
			_permission = permission;
			_privilege = privilege;
		}
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var roleId = Guid.Parse(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? Guid.Empty.ToString());

			if (roleId != Guid.Empty
				&& context.HttpContext.RequestServices.GetService(typeof(ICacheService)) is ICacheService _service)
			{
				var value = _service.GetRolePrivileges().Result.FirstOrDefault(x => x.RoleId == roleId
					&& x.PageId == Permissions.Attribute[(int)Permission]);

				if (value == null || (value.Privilege & (int)Privilege) != (int)Privilege)
				{
					context.Result = new ForbidResult();
				}
			}
		}
	}
}
