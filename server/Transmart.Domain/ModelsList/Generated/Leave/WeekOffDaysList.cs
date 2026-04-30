using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class WeekOffDaysList : BaseModel
    {
        public int Type { get; set; }
        public int? WeekDay { get; set; }
    }
}
