using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.Payroll
{
    public partial interface INewRegimeSlabService : IBaseService<NewRegimeSlab>
    {

    }

    public partial class NewRegimeSlabService : BaseService<NewRegimeSlab>, INewRegimeSlabService
    {
        public override async Task CustomValidation(NewRegimeSlab item, Result<NewRegimeSlab> result)
        {
            await base.CustomValidation(item, result);
            if (!result.HasError && item.IncomeTo <= item.IncomeFrom)
            {
                result.AddMessageItem(new MessageItem
                    (nameof(NewRegimeSlab.IncomeTo), Resource.IncomeTo_Greater_Than_IncomeFrom));
            }
            var a = await UOW.GetRepositoryAsync<NewRegimeSlab>().GetCountAsync(x => x.ID != item.ID 
                                                && x.IncomeFrom < item.IncomeTo && item.IncomeFrom < x.IncomeTo);
            if (a > 0)
            {
                result.AddMessageItem(new MessageItem(Resource.Slab_Already_Exists));
            }
        }
    }
}
