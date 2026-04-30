using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Helpdesk.Request
{
    public class TicketLogModel : BaseModel
    {
        public Guid TicketId { get; set; }
        public Guid RepliedById { get; set; }
        public string Mail { get; set; }
        public string Response { get; set; }
        public DateTime RepliedOn { get; set; }
        public byte TypeOfLog { get; set; }
        public string ToWorkMail { get; set; }
        public string TicketNo { get; set; }
        public Guid? TicketStatusId { get; set; }
        public Guid? AssignedToId { get; set; }
		public string RaisedByEmpMail { get; set; }
		public ICollection<TicketLogRecipientsModel> Recipients { get; set; }
		public string RepliedBy { get; set; }
    }
   
   
}
