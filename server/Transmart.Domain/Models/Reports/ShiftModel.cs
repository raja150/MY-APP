using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
   public class ShiftModel :BaseModel
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public DateTime DOJ { get; set; }
        public string Shift { get; set; }
        public string WeekOff { get; set; }
    }
}
