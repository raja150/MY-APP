using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class OldRegimeSlabList : BaseModel
    {
        public int IncomeFrom { get; set; }
        public int IncomeTo { get; set; }
        public int TaxRate { get; set; }
    }
}
