using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Leave
{
   public class LeaveBalanceModel : BaseModel
    {
        public Guid EmployeeId { get; set; }

        public Guid LeaveTypeId { get; set; }

        public DateTime LeavesAddedOn { get; set; }

        public DateTime LeavesExpiredOn { get; set; }

        public decimal LeavesAdded { get; set; }

        public decimal LeavesAvailable { get; set; }

        public decimal LeavesUsed { get; set; }
		public string LeaveTypeName { get; set; }
		public decimal Leaves { get; set; }

    }
}
