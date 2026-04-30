using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.List
{
    public class ApplyLeaveList : BaseModel
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        public string LeaveType { get; set; }
		public string LeaveTypes { get; set; }
        public string Status { get; set; }
        public decimal NoOfLeaves { get; set; }
		public string ApprovedBy { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
