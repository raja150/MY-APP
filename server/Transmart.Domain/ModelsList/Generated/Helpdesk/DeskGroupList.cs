using System;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class DeskGroupList : BaseModel
    {
        public string Name { get; set; }
        public int CanEditTicket { get; set; }
        public int CanPostReply { get; set; }
        public int CanCloseTicket { get; set; }
        public int CanAssignTicket { get; set; }
        public int CanTransferTicket { get; set; }
        public int CanDeleteTicket { get; set; }
        public bool Status { get; set; }
    }
}
