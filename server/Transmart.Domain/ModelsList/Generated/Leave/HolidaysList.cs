using System;

namespace TranSmart.Domain.Models.Leave
{
    public partial class HolidaysList : BaseModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
    }
}
