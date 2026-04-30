using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EarningComponentList : BaseModel
    {
        public int EarningType { get; set; }
        public string Name { get; set; }
        public bool ESIContribution { get; set; }
        public bool ShowInPayslip { get; set; }
        public bool HideWhenZero { get; set; }
        public bool Status { get; set; }
    }
}
