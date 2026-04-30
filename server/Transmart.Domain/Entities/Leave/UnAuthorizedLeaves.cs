using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
	[Table("LM_UnAuthorizedLeaves")]
	public class UnAuthorizedLeaves : DataGroupEntity
	{
		public Guid EmployeeId { get; set; }
		public Organization.Employee Employee { get; set; }
		public Guid RefId { get; set; }
		public DateTime Date { get; set; }
		public int LeaveStatus { get; set; }
	}
}
