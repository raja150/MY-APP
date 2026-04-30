using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Request
{
    public class ApplyLeaveRequest : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string EmergencyContNo { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        public bool FromHalf { get; set; }
        public bool ToHalf { get; set; }
        public string Reason { get; set; }
        public int? Status { get; set; }
        public decimal NoOfLeaves { get; set; }
		public string LeaveTypes { get; set; }
        public string RejectReason { get; set; }
		public bool IsPlanned { get; set; }
		public ICollection<ApplyLeaveTypeRequest> ApplyLeaveType { get; set; }
	}

}

