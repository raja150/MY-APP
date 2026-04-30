using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class OrganizationsList : BaseModel
    {
        public string Name { get; set; }
        public int ProbationPeriodType { get; set; }
        public int MonthStartDay { get; set; }
        public bool Status { get; set; }
    }
}
