using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;

namespace TranSmart.Service.Leave
{
    public partial interface ILeaveSettingsService : IBaseService<LeaveSettings>
    {

    }

    public partial class LeaveSettingsService : BaseService<LeaveSettings>, ILeaveSettingsService
    {
        public override async Task CustomValidation(LeaveSettings item, Result<LeaveSettings> result)
        {
            var existing = await UOW.GetRepositoryAsync<LeaveSettings>().GetCountAsync(x => x.ID == item.ID);
            var records = await UOW.GetRepositoryAsync<LeaveSettings>().GetCountAsync();
            //(existing == 0 && records == 1) while adding a new record, total records should be zero.  
            //(existing == 1 && records > 1) while updating a record, total records should be not more than one.
            if ((existing == 0 && records == 1) || (existing == 1 && records > 1))
            {
                result.AddMessageItem(new MessageItem(Resource.Only_One_Record_Is_Accepted));
            }
        }

    }
}
