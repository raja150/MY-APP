using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
    public partial interface IOldRegimeSlabService : IBaseService<OldRegimeSlab>
    {

    }

    public partial class OldRegimeSlabService : BaseService<OldRegimeSlab>, IOldRegimeSlabService
    {
        public override async Task CustomValidation(OldRegimeSlab item, Result<OldRegimeSlab> result)
        {
            await base.CustomValidation(item, result);
            if(!result.HasError &&item.IncomeTo <= item.IncomeFrom)
            {
                result.AddMessageItem(new MessageItem
                    (nameof(OldRegimeSlab.IncomeTo), Resource.IncomeTo_Greater_Than_IncomeFrom));
            }
            var a = await UOW.GetRepositoryAsync<OldRegimeSlab>().GetCountAsync(x => x.ID != item.ID &&x.IncomeFrom < item.IncomeTo && item.IncomeFrom < x.IncomeTo);
            if (a > 0)
            {
                result.AddMessageItem(new MessageItem(Resource.Slab_Already_Exists));
            }
        }
    }
}
