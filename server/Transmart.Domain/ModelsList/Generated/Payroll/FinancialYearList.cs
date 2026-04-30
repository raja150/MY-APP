using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class FinancialYearList : BaseModel
    {
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string Name { get; set; }
        public bool Closed { get; set; }
    }
}
