using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Model
{
	public class ApplyLeaveTypeModel : BaseModel
	{
		public Guid ApplyLeaveId { get; set; }
		public Guid LeaveTypeId { get; set; }
		public bool IsDefaultPayOff { get; set; }
		public decimal NoOfLeaves { get; set; }
		public string LeaveTypeName { get; set; }
	}
}
