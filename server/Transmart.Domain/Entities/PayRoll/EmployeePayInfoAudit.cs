using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.PayRoll
{
    [Table("EmployeePayInfo_Audit")]
    public class EmployeePayInfoAudit : AuditLogEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string PayType { get; set; } //Bank Transfer and Online
        public string Bank { get; set; }
		public string BankName { get; set; }
		public string IFSCCode { get; set; }
		public string AccountNo { get; set; } 
    }
}
