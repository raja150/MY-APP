using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class ProfessionalTaxSlabList : BaseModel
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Amount { get; set; }
    }
}
