using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_LeaveBalance")]
    public class LeaveBalance : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }

        public Entities.Organization.Employee Employee { get; set; }

        public Guid LeaveTypeId { get; set; }

        public Entities.Leave.LeaveType LeaveType { get; set; }

        public int Type { get; set; }

        public DateTime LeavesAddedOn { get; set; }

        public decimal Leaves { get; set; }

        public Guid? PreconsumableLeaveId { get; set; }

        public ApprovedLeaves PreconsumableLeave { get; set; }

        public Guid? ApplyLeaveId { get; set; }

        public ApplyLeave ApplyLeave { get; set; } 

        public Guid? ApplyCompensatoryId { get; set; }

        public Leave.ApplyCompo ApplyCompensatory { get; set; }

        public Guid? CustomizedBalId { get; set; }

        public Leave.AdjustLeave CustomizedBal { get; set; }
		public DateTime EffectiveFrom { get; set; }
		public DateTime EffectiveTo { get; set; }
	}
}
