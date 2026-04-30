using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class LocationList : BaseModel
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Shift { get; set; }
        public string WeekOffSetup { get; set; }
        public string WorkHoursSetting { get; set; }
        public bool Status { get; set; }
    }
}
