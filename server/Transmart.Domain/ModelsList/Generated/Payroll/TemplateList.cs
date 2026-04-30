using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class TemplateList : BaseModel
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
