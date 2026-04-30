using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
	[Table("LM_LeaveRequestLeaveType")]
	public class ApplyLeaveType : DataGroupEntity
	{
		public Guid ApplyLeaveId { get; set; }
		public ApplyLeave ApplyLeave { get; set; }
		public Guid LeaveTypeId { get; set; }
		public LeaveType LeaveType { get; set; }
		public decimal NoOfLeaves { get; set; }
	}
}
