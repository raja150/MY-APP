using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class OtherSectionsList : BaseModel
    {
        public string Name { get; set; }
        public int Limit { get; set; }
        public bool Status { get; set; }
    }
}
