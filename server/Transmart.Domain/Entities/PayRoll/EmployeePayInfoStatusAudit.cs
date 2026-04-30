using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.PayRoll
{
	[Table("EmployeePayInfoStatus_Audit")]
	public class EmployeePayInfoStatusAudit : AuditLogEntity
	{
		public Guid EmployeeId { get; set; }
		public Employee Employee { get; set; }
		public string Status { get; set; }
	}
}
