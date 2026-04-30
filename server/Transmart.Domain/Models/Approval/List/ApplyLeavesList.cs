using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class ApplyLeavesList : BaseModel
    {
		public Guid EmployeeId { get; set; }
		public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string LeaveTypes { get; set; }
        public string EmergencyContNo { get; set; } 
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public decimal NoOfLeaves { get; set; }
		public decimal LopDays { get; set; }
		public string ApprovedBy { get; set; }
        public Guid ApprovedById { get; set; }
		public bool IsPlanned { get; set; }
		public string ReportingTo { get; set; }

	}
}
