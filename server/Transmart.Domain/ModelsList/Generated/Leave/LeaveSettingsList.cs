using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class LeaveSettingsList : BaseModel
    {
        public int HourCalculation { get; set; }
        public int? FullDayHours { get; set; }
        public int? HalfDayHours { get; set; }
        public string CompCreditTo { get; set; }
        public string CompoLeaveType { get; set; }
    }
}
