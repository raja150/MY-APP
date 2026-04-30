using System;
using System.Collections.Generic;
using System.Linq;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace TranSmart.Service.AppSettings
{
	public partial interface IRolesService : IBaseService<Role>
	{
		bool Validate(string name);
		Task<IEnumerable<RolePrivilege>> RoleMenu(Guid RoleId);
		Task<IEnumerable<Page>> Pages();
		Task<IEnumerable<Group>> GetModules();
		Task<IEnumerable<Report>> GetReport(Guid moduleId);
		Task<IEnumerable<Role>> Roles();
	}

	public class RolesService : BaseService<Role>, IRolesService
	{
		public RolesService(IUnitOfWork uow) : base(uow)
		{

		}

		public override async Task<Role> GetById(Guid id)
		{
			Role entity = await UOW.GetRepositoryAsync<Role>().SingleAsync(x => x.ID == id);
			entity.Pages = (await UOW.GetRepositoryAsync<RolePrivilege>().GetAsync(x => x.RoleId == id,
				orderBy: o => o.OrderBy(d => d.Page.Group.DisplayOrder).ThenBy(dd => dd.Page.DisplayOrder),
				include: i => i.Include(x => x.Page).ThenInclude(x => x.Group))).ToList();
			entity.Reports = (await UOW.GetRepositoryAsync<RoleReportPrivilege>().GetAsync(x => x.RoleId == id,
				   orderBy: o => o.OrderBy(d => d.Report.Module.Name).ThenBy(dd => dd.Report.Label),
				   include: i => i.Include(x => x.Report).ThenInclude(x => x.Module))).ToList();
			return entity;
		}
		public bool Validate(string name)
		{
			//Role entity = _UOW.GetRepository<Role>().Single(x => x. == name);
			//if (entity == null)
			//{
			//    return false;
			//}
			return true;
		}

		public async Task<IEnumerable<RolePrivilege>> RoleMenu(Guid RoleId)
		{
			IEnumerable<RolePrivilege> privilege = await UOW.GetRepositoryAsync<RolePrivilege>().GetAsync(
				predicate: x => x.RoleId == RoleId && x.Privilege > 0,
				include: x => x.Include(i => i.Page).ThenInclude(o => o.Group),
				orderBy: o => o.OrderBy(d => d.Page.Group.DisplayOrder).ThenBy(dd => dd.Page.DisplayOrder));

			return privilege;
		}



		public async Task<IEnumerable<Page>> Pages()
		{
			return await UOW.GetRepositoryAsync<Page>().GetAsync(
				include: x => x.Include(i => i.Group),
				orderBy: o => o.OrderBy(d => d.Group.DisplayOrder).ThenBy(d => d.DisplayOrder));
		}

		public override async Task CustomValidation(Role item, Result<Role> result)
		{
			await base.CustomValidation(item, result);
			if (await UOW.GetRepositoryAsync<Role>().HasRecordsAsync(x => x.ID == item.ID && !x.CanEdit))
			{
				result.AddMessageItem(new MessageItem("Sorry!, You can not edit this role."));
			}
			if(await UOW.GetRepositoryAsync<Role>().HasRecordsAsync(x => x.ID != item.ID && x.Name == item.Name)) {
				result.AddMessageItem(new MessageItem(nameof(Role.Name), "Sorry!, Entered name is already exist"));
			}
		}
		public async Task<IEnumerable<Group>> GetModules()
		{
			return await UOW.GetRepositoryAsync<Group>().GetAsync();
		}
		public async Task<IEnumerable<Report>> GetReport(Guid moduleId)
		{
			return await UOW.GetRepositoryAsync<Report>().GetAsync(
				predicate: p => p.ModuleId == moduleId,
				include: i => i.Include(x => x.Module));
		}
		public override async Task<Result<Role>> UpdateAsync(Role item)
		{
			Role role = await UOW.GetRepositoryAsync<Role>().SingleAsync(x => x.ID == item.ID,
			   include: o => o.Include(x => x.Pages).Include(x => x.Reports));
			if (role != null)
			{
				role.Name = item.Name;
				foreach (var p in item.Pages)
				{
					RolePrivilege page = role.Pages.FirstOrDefault(x => x.PageId == p.PageId);
					if (page == null)
					{
						role.Pages.Add(new RolePrivilege { PageId = p.PageId, RoleId = item.ID, Privilege = p.Privilege });
					}
					else
					{
						page.Privilege = p.Privilege;
					}
				}
				foreach (var r in item.Reports)
				{
					RoleReportPrivilege report = role.Reports.FirstOrDefault(x => x.ReportId == r.ReportId);
					if (report == null)
					{
						role.Reports.Add(new RoleReportPrivilege { ReportId = r.ReportId, RoleId = item.ID, Privilege = r.Privilege });
					}
					else
					{
						report.Privilege = r.Privilege;
					}
				}
			}
			return await base.UpdateAsync(role);
		}
		public async Task<IEnumerable<Role>> Roles()
		{
			return await UOW.GetRepositoryAsync<Role>().GetAsync(
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
	}

}
