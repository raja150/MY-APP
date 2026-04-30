using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities;

namespace TranSmart.Service.Reports
{
    public interface IAppReportService : IBaseService<Report>
    {
        Task<IEnumerable<ReportModule>> GetModules();
		Task<IEnumerable<Domain.Entities.AppSettings.RoleReportPrivilege>> GetRoleReports(Guid moduleId, Guid roleId);
		Task<IEnumerable<Domain.Entities.Report>> GetReports(Guid moduleId);
        Task<IEnumerable<Domain.Entities.Report>> GetReports();
    }
    public class AppReportService : BaseService<Report>, IAppReportService
    {
        public AppReportService(IUnitOfWork uow) : base(uow)
        {

        }
        public async Task<IEnumerable<ReportModule>> GetModules()
        {
            return await UOW.GetRepositoryAsync<ReportModule>().GetAsync(orderBy: x => x.OrderBy(o => o.Label));
        }
		public async Task<IEnumerable<Domain.Entities.Report>> GetReports(Guid moduleId)
		{
			return await UOW.GetRepositoryAsync<Domain.Entities.Report>().GetAsync(
				x => x.ModuleId == moduleId, include: i => i.Include(x => x.Module), orderBy: x => x.OrderBy(o => o.Label));
		}

		public async Task<IEnumerable<Domain.Entities.AppSettings.RoleReportPrivilege>> GetRoleReports(Guid moduleId, Guid roleId)
		{
			return await UOW.GetRepositoryAsync<Domain.Entities.AppSettings.RoleReportPrivilege>().GetAsync(
				x => x.Report.ModuleId == moduleId && x.RoleId == roleId, include: i => i.Include(x => x.Report), orderBy: x => x.OrderBy(o => o.Report.Label));
		}

		public async Task<IEnumerable<Report>> GetReports()
        {
            return await UOW.GetRepositoryAsync<Domain.Entities.Report>().GetAsync(
                include: x => x.Include(i => i.Module),
                  orderBy: x => x.OrderBy(o => o.Module.Name).ThenBy(o => o.Label));
        }
    }
}
