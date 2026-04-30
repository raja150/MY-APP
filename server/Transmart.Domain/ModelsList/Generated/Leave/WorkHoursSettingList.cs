using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class WorkHoursSettingList : BaseModel
    {
        public string Name { get; set; }
        public int FullDayMinutes { get; set; }
        public int HalfDayMinutes { get; set; }
        public bool Status { get; set; }
    }
}
