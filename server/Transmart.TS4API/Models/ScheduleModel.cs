using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transmart.TS4API.Models
{
    public partial class ScheduleModel 
    {
        public Guid TS4ID { get; set; }
        public DateTime ScheduleFrom { get; set; }
        public DateTime? ScheduleTo { get; set; }
        public int InTime { get; set; }
        public int OutTime { get; set; }
        public int OutNextDay { get; set; }
        public int BreakTime { get; set; }
        public int NoOfBreaks { get; set; }
    }
}
