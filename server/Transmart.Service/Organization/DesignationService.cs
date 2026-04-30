using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
    public partial interface IDesignationService : IBaseService<Designation>
    {
        Task<IPaginate<Designation>> GetAllWeekOffDesign(BaseSearch search);
        Task<Result<Designation>> UpdateWeekoff(DesignationAllocationModel item);

        Task<Result<Designation>> DeleteWeekOffSetup(Designation item);

    }
    public partial class DesignationService : BaseService<Designation>, IDesignationService
    {
        public async Task<IPaginate<Designation>> GetAllWeekOffDesign(BaseSearch search)
        {
            return await UOW.GetRepositoryAsync<Designation>().GetPageListAsync(
                predicate: x => x.WeekOffSetupId == search.RefId,
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Type", ascending: !search.IsDescend);
        }
		public override async Task OnAfterAdd(Designation item, Result<Designation> executionResult)
		{
			if (executionResult.IsSuccess)
			{
				await UOW.GetRepositoryAsync<Replication>().AddAsync(new Replication
				{
					DepartmentId= item.DepartmentId,
					Type = (byte)ReplicationType.Add,
					Category = (byte)ReplicationCategory.Designation,
					RefId = item.ID,
				});
				await UOW.SaveChangesAsync();
			}
			await base.OnAfterAdd(item, executionResult);
		}
		public override async Task OnAfterUpdate(Designation item, Result<Designation> executionResult)
		{
			if (executionResult.IsSuccess)
			{
				var replication = await UOW.GetRepositoryAsync<Replication>().SingleAsync(x => x.RefId == item.ID 
									&& x.Category == (byte)ReplicationCategory.Designation 
									&& x.Type == (byte)ReplicationType.Update);
				
				if(replication == null)
				{
					await UOW.GetRepositoryAsync<Replication>().AddAsync(new Replication
					{
						DepartmentId = item.DepartmentId,
						Type = (byte)ReplicationType.Update,
						Category = (byte)ReplicationCategory.Designation,
						RefId = item.ID,
					});
				}
				else
				{
					replication.Status = false;
					UOW.GetRepositoryAsync<Replication>().UpdateAsync(replication);
				}
				await UOW.SaveChangesAsync();
			}
			await base.OnAfterUpdate(item, executionResult);
		}

		public async Task<Result<Designation>> UpdateWeekoff(DesignationAllocationModel item)
        {
            Result<Designation> result = new Result<Designation>();
            var design = await UOW.GetRepositoryAsync<Designation>().SingleAsync(x => x.ID == item.DesignationId);

            if (design.WeekOffSetupId != null && design.ID == item.DesignationId)
            {
                result.AddMessageItem(new MessageItem(Resource.Week_Already_Exist));
                return result;
            }
            design.WeekOffSetupId = item.WeekOffSetupId;
            _ = await UpdateOnlyAsync(design);
            result.ReturnValue = design;
            result.IsSuccess = true;
            return result;
        }

        public async Task<Result<Designation>> DeleteWeekOffSetup(Designation item)
        {
            Result<Designation> result = new Result<Designation>();
            var designation = await UOW.GetRepositoryAsync<Designation>().SingleAsync(x => x.ID == item.ID);
            if (designation == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
                return result;
            }
            designation.WeekOffSetupId = null;
            _ = await UpdateOnlyAsync(designation);
            result.ReturnValue = designation;
            result.IsSuccess = true;
            return result;
        }
    }
}
