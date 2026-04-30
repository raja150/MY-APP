using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.Service.Payroll
{
    public partial interface IESIService : IBaseService<ESI>
    {

    }

    public partial class ESIService : BaseService<ESI>, IESIService
    {
        public override async Task CustomValidation(ESI item, Result<ESI> result)
        {
            if (await UOW.GetRepositoryAsync<ESI>().GetCountAsync(x => x.PaySettingsId == item.PaySettingsId
             && x.ID != item.ID) != 0)
            {
                result.AddMessageItem(new MessageItem("Duplicate settings are not allowed"));
            }
        }
    }
}
