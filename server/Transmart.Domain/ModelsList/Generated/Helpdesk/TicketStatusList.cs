using System;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class TicketStatusList : BaseModel
    {
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool IsClosed { get; set; }
        public bool Status { get; set; }
    }
}
