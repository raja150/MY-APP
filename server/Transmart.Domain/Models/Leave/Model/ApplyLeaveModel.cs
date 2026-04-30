using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Leave.Request;

namespace TranSmart.Domain.Models.Leave.Model
{
    public class ApplyLeaveModel:BaseModel
    {
		public string LeaveTypes { get; set; }
		public Guid EmployeeId { get; set; }
        public string EmployeeNo { get; set; }  
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string EmergencyContNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        public bool FromHalf { get; set; }
        public bool ToHalf { get; set; }
        public string Reason { get; set; }
        public string RejectReason { get; set; }
        public Guid LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public int Status { get; set; }
		public bool IsPlanned { get; set; }
		public ICollection<ApplyLeaveTypeModel> ApplyLeaveType { get; set; }
	}
}
