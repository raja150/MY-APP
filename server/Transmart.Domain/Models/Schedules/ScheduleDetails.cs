using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Schedules
{
   public class ScheduleDetails
    {
        public Guid EmployeeID { get; set; }
        public DateTime FromDate { get; set; }
        public int StartAt { get; set; }
        public int EndsAt { get; set; }
        public int BreakTime { get; set; }
        public int NoOfBreaks { get; set; }
        public int NextDayOut { get; set; }
        public int? LoginGraceTime { get; set; }
    }
}
