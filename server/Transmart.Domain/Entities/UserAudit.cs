using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities
{
    [Table("User_Audit")]
    public class UserAudit : AuditEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }  
        public int ActionType { get; set; }
        public string Description { get; set; }  
    }
}
