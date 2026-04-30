using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class DeductionComponentList : BaseModel
    {
        public int Deduct { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
