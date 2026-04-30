using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ApprovedLeavesList : BaseModel
    {
        public string Employee { get; set; }
        public string Reason { get; set; }
    }
}
