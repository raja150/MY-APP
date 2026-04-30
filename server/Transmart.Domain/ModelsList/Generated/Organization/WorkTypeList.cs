using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class WorkTypeList : BaseModel
    {
        public string Name { get; set; }
        public bool SalaryPaying { get; set; }
        public bool Status { get; set; }
    }
}
