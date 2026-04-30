using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities;

namespace TranSmart.Service
{
	public interface IApplicationAuditLogService : IBaseService<AplicationAuditLog>
	{
		Task GetAccesedUser(string user, string action, string ipAddress, string entity);
	}
	public class ApplicationAuditLogService : BaseService<AplicationAuditLog>, IApplicationAuditLogService
	{
		public ApplicationAuditLogService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task GetAccesedUser(string user, string action, string ipAddress, string entity)
		{
			await base.AddAsync(new AplicationAuditLog
			{
				AccesedAt = DateTime.Now,
				AccesedBy = user,
				IPAddress = ipAddress,
				Action = action,
				Entity = entity
			});
			await UOW.SaveChangesAsync();
		}
	}
}
