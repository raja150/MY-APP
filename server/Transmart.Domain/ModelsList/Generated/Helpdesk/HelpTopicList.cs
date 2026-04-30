using System;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class HelpTopicList : BaseModel
    {
        public string Name { get; set; }
        public int DueHours { get; set; }
        public int Priority { get; set; }
        public string Department { get; set; }
        public string TicketStatus { get; set; }
        public bool Status { get; set; }
    }
}
