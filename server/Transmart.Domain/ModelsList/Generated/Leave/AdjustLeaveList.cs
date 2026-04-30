using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class AdjustLeaveList : BaseModel
    {
        public string Employee { get; set; }
        public string LeaveType { get; set; }
        public decimal NewBalance { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}
