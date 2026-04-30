using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities
{
    [Table("AuditLog")]
    public class AplicationAuditLog : BaseEntity
    {
        public string Entity { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
        public string AccesedBy { get; set; }
        public DateTime AccesedAt { get; set; }
    }
}
