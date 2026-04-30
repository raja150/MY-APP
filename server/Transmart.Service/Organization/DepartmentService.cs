using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
	public partial interface IDepartmentService : IBaseService<Department>
	{
		Task<IPaginate<Department>> GetAllWeekOffDepts(BaseSearch search);
		Task<Result<Department>> UpdateWeekoff(DepartmentAllocationModel item);
		Task<Result<Department>> DeleteWeekOffSetup(DepartmentAllocationModel item);
	}
	public partial class DepartmentService : BaseService<Department>, IDepartmentService
	{

		public override async Task CustomValidation(Department item, Result<Department> result)
		{

			if (!string.IsNullOrEmpty(item.Email) && !Regex.IsMatch(item.Email, Resource.Regx_Email_Validation))
			{
				result.AddMessageItem(new MessageItem
					 (nameof(Department.Email), Resource.Invalid_Email_Format));
			}
			else
			{
				if (!string.IsNullOrEmpty(item.Email) &&
						await UOW.GetRepositoryAsync<Department>().HasRecordsAsync(x => x.Email == item.Email && x.ID != item.ID))
				{
					result.AddMessageItem(new MessageItem
						  (nameof(Department.Email), Resource.Email_Already_Exist));
				}
			}

		}
		public async Task<IPaginate<Department>> GetAllWeekOffDepts(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<Department>().GetPageListAsync(
				predicate: x => x.WeekOffSetupId == search.RefId,
				index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Type", ascending: !search.IsDescend);
		}

		public async Task<Result<Department>> UpdateWeekoff(DepartmentAllocationModel item)
		{
			Result<Department> result = new Result<Department>();
			var department = await UOW.GetRepositoryAsync<Department>().SingleAsync(x => x.ID == item.DepartmentId);
			if (department.WeekOffSetupId != null && department.ID == item.DepartmentId)
			{
				result.AddMessageItem(new MessageItem(Resource.Week_Already_Exist));
				return result;
			}
			department.WeekOffSetupId = item.WeekOffSetupId;
			_ = await UpdateOnlyAsync(department);
			result.ReturnValue = department;
			result.IsSuccess = true;

			return result;
		}

		public async Task<Result<Department>> DeleteWeekOffSetup(DepartmentAllocationModel item)
		{
			Result<Department> result = new Result<Department>();
			var department = await UOW.GetRepositoryAsync<Department>().SingleAsync(x => x.ID == item.ID);
			if (department == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
				return result;
			}
			department.WeekOffSetupId = null;
			_ = await UpdateOnlyAsync(department);
			result.ReturnValue = department;
			result.IsSuccess = true;
			return result;
		}
	}
}
