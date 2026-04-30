using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class TemplateEarningList : BaseModel
    {
        public string Component { get; set; }
        public int Type { get; set; }
        public int? Percentage { get; set; }
        public int? Amount { get; set; }
        public int? PercentOn { get; set; }
        public string PercentOnComp { get; set; }
    }
}
