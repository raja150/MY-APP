using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using TranSmart.API.Services;
using TranSmart.Core;
using TranSmart.Domain.Models.Cache; 

namespace TranSmart.API.Extensions
{
    public class ApiReportAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ReportPermission _permission;
		public ReportPermission Permission { get { return _permission; } }

		public ApiReportAuthorizeAttribute(ReportPermission permission)
		{
			_permission = permission;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
        {
			var roleId = Guid.Parse(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? Guid.Empty.ToString());

			ICacheService _service = context.HttpContext.RequestServices.GetService(typeof(ICacheService)) as ICacheService;
			if (roleId != Guid.Empty && _service != null)
			{
				RoleReportPrivilegeCache value = _service.GetRoleReportPrivileges().Result.FirstOrDefault(x => x.RoleId == roleId
					&& x.ReportId == Permissions.ReportAttribute[(int)_permission]);

				if (value == null || !value.CanView)
				{
					context.Result = new ForbidResult();
				}
			}
		}
    }

}
