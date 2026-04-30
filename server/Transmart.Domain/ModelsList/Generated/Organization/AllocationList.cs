using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class AllocationList : BaseModel
    {
        public string Shift { get; set; }
        public string WeekOffSetup { get; set; }
        public string WorkHoursSetting { get; set; }
    }
}
