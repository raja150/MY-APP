using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.DailyEvents
{
    [Table("Daily_EventLog")]
    public class DailyEvent : DataGroupEntity
    {
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }

        public string EventName { get; set; }
        public string ErrMSG { get; set; }
        public string Branch { get; set; }
    }
}
