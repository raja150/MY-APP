using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EmpBonusList : BaseModel
    {
        public string Employee { get; set; }
        public int Amount { get; set; }
        public DateTime ReleasedOn { get; set; }
    }
}
