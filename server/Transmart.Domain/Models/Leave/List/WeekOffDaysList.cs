using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.List
{
    public class WeekOffDaysList : BaseModel
    {  
        public int? WeekDay { get; set; } //Week Day 
        public string WeekNoInMonth { get; set; } //Week Number
        public DateTime? WeekDate { get; set; }

        public string Status { get; set; } //Off        Present
        public string Type { get; set; } //Weeks in a month  Weeks in a year 	Week Date
        public string WeekInYear { get; set; } // All        On even weeks        On odd weeks

    }
}
