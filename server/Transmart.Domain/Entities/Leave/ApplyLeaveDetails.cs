using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
	[Table("LM_LeaveRequestDetails")]
	public class ApplyLeaveDetails : DataGroupEntity
	{
		public Guid ApplyLeaveId { get; set; }
		public ApplyLeave ApplyLeave { get; set; }
		public Guid LeaveTypeId { get; set; }
		public LeaveType LeaveType { get; set; }
		[Required]
		public DateTime LeaveDate { get; set; }
		public bool IsHalfDay { get; set; }
		public bool IsFirstHalf { get; set; }
		public decimal LeaveCount { get; set; }
	}
}
