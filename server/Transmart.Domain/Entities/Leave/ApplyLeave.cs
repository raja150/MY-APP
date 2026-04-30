
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_LeaveRequest")]
    public partial class ApplyLeave : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid? ApprovedById { get; set; }
        public Organization.Employee ApprovedBy { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal NoOfLeaves { get; set; }
		public string LeaveTypes { get; set; }
        [StringLength(1024)]
        public string EmergencyContNo { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }         
        public bool FromHalf { get; set; }
        public bool ToHalf { get; set; } 
        [StringLength(1024)]
        public string Reason { get; set; }
        public byte Status { get; set; }
        [StringLength(1024)]
        public string RejectReason { get; set; }
		public bool IsPlanned { get; set; }
		public ICollection<ApplyLeaveType> ApplyLeaveType { get; set; }

		public void Update(ApplyLeave other)
		{
			FromDate= other.FromDate;
			ToDate = other.ToDate;
			FromHalf= other.FromHalf;
			ToHalf= other.ToHalf;
			Reason= other.Reason;
			EmergencyContNo= other.EmergencyContNo;
			LeaveTypes = other.LeaveTypes;
			NoOfLeaves = other.NoOfLeaves;
		}
    }
}
