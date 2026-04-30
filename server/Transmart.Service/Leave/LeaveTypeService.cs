using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;

namespace TranSmart.Service.Leave
{
    public partial interface ILeaveTypeService : IBaseService<LeaveType>
    {
        void DeptAndDesignValidation(LeaveType item, Result<LeaveType> executionResult);
		Task<LeaveType> GetDefaultPayOffLeaveType();
		Task<IEnumerable<LeaveType>> GetPaidLeaveTypeList();

	}

    public partial class LeaveTypeService : BaseService<LeaveType>, ILeaveTypeService
    {
        public override async Task CustomValidation(LeaveType item, Result<LeaveType> result)
        {
            await base.CustomValidation(item, result);
			if (item.MinLeaves > item.MaxLeaves)
			{
				result.AddMessageItem(new MessageItem(nameof(LeaveType.MaxLeaves), Resource.MAX_LEAVES_GREATER_THEN_OR_EQUAL_TO_MIN_LEAVES));
			}
			if (await UOW.GetRepositoryAsync<LeaveType>().HasRecordsAsync(x => x.Name == item.Name && x.Status && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(nameof(LeaveType.Name), Resource.Name_already_exists));
			}
			if (await UOW.GetRepositoryAsync<LeaveType>().HasRecordsAsync(x => x.DefaultPayoff && item.DefaultPayoff && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Default_Pay_Off_LeaveType_already_exists));
			}
			//To Verify Same Department Or Designation Or Location is Selected  In LeaveType Exceptions
			DeptAndDesignValidation(item, result);
        }

        public void DeptAndDesignValidation(LeaveType item, Result<LeaveType> executionResult)
        {
            string[] Department = item.Department.Split(","); string[] ExDepartment = item.ExDepartment.Split(",");
            string[] Designation = item.Designation.Split(","); string[] ExDesignation = item.ExDesignation.Split(",");
            string[] Location = item.Location.Split(","); string[] ExLocation = item.ExLocation.Split(",");
            if (!string.IsNullOrEmpty(item.Department) && Department.Intersect(ExDepartment).Any())
            {
                executionResult.AddMessageItem(new MessageItem(nameof(LeaveType.Department), Resource.Department_There_In_Exception));
            }
            if (!string.IsNullOrEmpty(item.Designation) && Designation.Intersect(ExDesignation).Any())
            {
                executionResult.AddMessageItem(new MessageItem(nameof(LeaveType.Designation), Resource.Designation_There_In_Exception));
            }
            if (!string.IsNullOrEmpty(item.Location) && Location.Intersect(ExLocation).Any())
            {
                executionResult.AddMessageItem(new MessageItem(nameof(LeaveType.Location), Resource.Location_There_In_Exception));
            }
        }
		public async Task<LeaveType> GetDefaultPayOffLeaveType()
		{
			return await UOW.GetRepositoryAsync<LeaveType>().SingleAsync(x => x.DefaultPayoff == true);
		}
		public async Task<IEnumerable<LeaveType>> GetPaidLeaveTypeList()
		{
			return await UOW.GetRepositoryAsync<LeaveType>().GetAsync(x => x.DefaultPayoff == false);
		}
	}
}
