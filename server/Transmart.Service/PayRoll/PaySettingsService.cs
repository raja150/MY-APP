using System;
using TranSmart.Core.Result;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TranSmart.Data.Paging;
using System.Linq;
using TranSmart.Domain.Models;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using System.Threading.Tasks;

namespace TranSmart.Service.Payroll
{
    public partial interface IPaySettingsService : IBaseService<PaySettings>
    {

    }

    public partial class PaySettingsService : BaseService<PaySettings>, IPaySettingsService
    {
        public override async Task CustomValidation(PaySettings item, Result<PaySettings> result)
        {
            await base.CustomValidation(item, result);
            int count = await UOW.GetRepositoryAsync<PaySettings>().GetCountAsync(x => x.OrganizationId == item.OrganizationId);
            if (count > 0)
            {
                result.AddMessageItem(new MessageItem
                    (nameof(PaySettings.OrganizationId), Resource.Settings_For_This_Origination_Already_Added_));
            }
        }
    }
}
