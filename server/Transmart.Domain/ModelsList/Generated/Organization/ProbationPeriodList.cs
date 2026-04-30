using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class ProbationPeriodList : BaseModel
    {
        public string Name { get; set; }
        public int ProbationPeriodType { get; set; }
        public int ProbationPeriodTime { get; set; }
        public bool Status { get; set; }
    }
}
