using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class WeekOffSetupList : BaseModel
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
