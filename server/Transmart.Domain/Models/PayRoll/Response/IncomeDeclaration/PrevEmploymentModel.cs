using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class PrevEmploymentModel : BaseModel
    {
        public Guid DeclarationId { get; set; }
        public int IncomeAfterException { get; set; }
        public int IncomeTax { get; set; }
        public int ProfessionalTax { get; set; }
        public int ProvisionalFund { get; set; }
        public int EncashmentExceptions { get; set; }
    }
}
