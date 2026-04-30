using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.PayRoll
{
	[Table("PS_EmployeePayInfo")]
	public partial class EmployeePayInfo : DataGroupEntity
	{

		public Guid EmployeeId { get; set; }

		public Entities.Organization.Employee Employee { get; set; }
		[Required]
		public int PayMode { get; set; }

		public Guid BankId { get; set; }

		public Entities.Payroll.Bank Bank { get; set; }
		public string BankName { get; set; }
		public string IFSCCode { get; set; }
		[StringLength(20)]
		public string AccountNo { get; set; }
	}
}
