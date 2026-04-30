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
    public partial interface IProfessionalTaxService : IBaseService<ProfessionalTax>
    {

    }

    public partial class ProfessionalTaxService : BaseService<ProfessionalTax>, IProfessionalTaxService
    {
        public override async Task CustomValidation(ProfessionalTax item, Result<ProfessionalTax> result)
        {
            if (await UOW.GetRepositoryAsync<ProfessionalTax>().HasRecordsAsync(x => x.StateId == item.StateId && x.ID != item.ID))
            {
                result.AddMessageItem(new MessageItem(nameof(ProfessionalTax.StateId), Resource.State_Name_already_exists));
            }
        }
    }
}
