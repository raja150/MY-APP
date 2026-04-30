using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;

namespace TranSmart.Service
{
	public partial interface IReplicationService : IBaseService<Replication>
	{
		Task<Result<Replication>> UpdateReplications(List<Guid> items);
		Task<IEnumerable<Replication>> GetRelicationData(Guid departmentId);
	}
	public partial class ReplicationService : BaseService<Replication>, IReplicationService
	{
		public ReplicationService(IUnitOfWork uow) : base(uow)
		{
		}
		public async Task<IEnumerable<Replication>> GetRelicationData(Guid departmentId)
		{
			return await UOW.GetRepositoryAsync<Replication>().GetAsync(x => !x.Status && x.DepartmentId == departmentId,
			orderBy: o => o.OrderBy(x => x.Type).ThenBy(x => x.Category));
		}
		public async Task<Result<Replication>> UpdateReplications(List<Guid> items)
		{
			var result = new Result<Replication>();
			foreach (var id in items)
			{
				var entity = await UOW.GetRepositoryAsync<Replication>().SingleAsync(x => x.ID == id);
				if (entity != null)
				{
					entity.Status = true;
					UOW.GetRepositoryAsync<Replication>().UpdateAsync(entity);
				}
				else
				{
					result.AddMessageItem(new MessageItem(Resource.There_Is_No_Replication_Data));
					return result;
				}
			}
			try
			{
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex.Message));
			}

			return result;
		}
	}
}
