using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.HelpDesk
{
    [Table("SS_TicketLogRecipients")]
    public class TicketLogRecipients: DataGroupEntity
    {
        public Guid TicketLogId { get; set; }
        public TicketLog TicketLog { get; set; }    
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
    }
}
