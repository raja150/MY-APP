using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class TeamList : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Shift { get; set; }
        public string WeekOffSetup { get; set; }
        public string WorkHoursSetting { get; set; }
        public bool Status { get; set; }
    }
}
