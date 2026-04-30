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
    public partial interface IProfessionalTaxSlabService : IBaseService<ProfessionalTaxSlab>
    {

    }

    public partial class ProfessionalTaxSlabService : BaseService<ProfessionalTaxSlab>, IProfessionalTaxSlabService
    {
        public override async Task CustomValidation(ProfessionalTaxSlab item, Result<ProfessionalTaxSlab> result)
        {
            await base.CustomValidation(item, result);

            var a = await UOW.GetRepositoryAsync<ProfessionalTaxSlab>().GetCountAsync(
                x => x.ProfessionalTaxId == item.ProfessionalTaxId && item.From >= x.From && item.To <= x.To && x.ID != item.ID);

            if (a > 0)
            {
                result.AddMessageItem(new MessageItem(Resource.Slab_Already_Exists));
            }
        }

    }
}


