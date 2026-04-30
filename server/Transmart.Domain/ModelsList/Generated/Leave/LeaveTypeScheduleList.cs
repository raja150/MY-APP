using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class LeaveTypeScheduleList : BaseModel
    {
        public int AccType { get; set; }
        public int AccOnDay { get; set; }
        public int NoOfDays { get; set; }
        public int ResetType { get; set; }
    }
}
