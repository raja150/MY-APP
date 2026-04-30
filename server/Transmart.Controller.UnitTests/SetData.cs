using System;
using System.Collections.Generic;
using System.Linq;
using TranSmart.API.Domain.Models;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Models.AppSettings;
using TranSmart.Domain.Models.Settings;

namespace TranSmart.AR.Controller.UnitTest.Data
{
	public static class SetData
	{
		public static IEnumerable<Role> RolesData()
		{
			var roles = new List<Role>()
			{
				new Role { CanEdit = true,ID = Guid.NewGuid() ,Name = "Administrator"},
				new Role { CanEdit = true,ID = Guid.NewGuid() ,Name = "User"}

			}.AsQueryable();

			return roles;
		}
		public static RoleModel RoleRequest()
		{
			var request = new RoleModel
			{
				CanEdit = true,
				Name = "Shiva",
				Pages = new List<RolePrivilegeModel>
				{
					new RolePrivilegeModel
					{
						Name ="MyClaims",DisplayName = "My Claims",Module ="Claims"
					},
					new RolePrivilegeModel
					{
						Name ="Audit",DisplayName = "Audit", Module ="Claims"
					}
				},
				Reports = new List<RoleReportPrivilegeModel>
				{
					new RoleReportPrivilegeModel
					{
						Privilege = true,Module ="ARReports",Name ="WeeklyReport",Label ="Weekly Report"
					},
					new RoleReportPrivilegeModel
					{
						Privilege = true,Module ="ARReports",Name ="Aging",Label ="Aging Report"
					}
				}

			};
			return request;
		}
	}
}
