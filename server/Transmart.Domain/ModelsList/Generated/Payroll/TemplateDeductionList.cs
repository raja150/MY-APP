using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class TemplateDeductionList : BaseModel
    {
        public string Component { get; set; }
        public int Amount { get; set; }
    }
}
