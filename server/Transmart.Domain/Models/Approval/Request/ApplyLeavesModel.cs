using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TranSmart.Domain.Enums;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class ApplyLeavesModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? ApprovedById { get; set; } 
        public Guid LeaveTypeId { get; set; } 
        [Required]
        public string EmergencyContNo { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; } 
        public bool FromHalf { get; set; }
        public bool ToHalf { get; set; }
        [Required]
        public string Reason { get; set; }
        public string RejectReason { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNo { get; set; }
        public string LeaveType { get; set; }
        public byte Status { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string StatusValue { get { return Enum.GetName(typeof(ApplyLeaveSts), Status); } }
        public decimal NoOfLeaves { get; set; }
    }
    public class ApplyLeavesModelValidator : AbstractValidator<ApplyLeavesModel>
    {
        public ApplyLeavesModelValidator()
        {
            RuleFor(m => m.EmergencyContNo).NotEmpty().WithName("Emergency Contact No");
            RuleFor(c => c.EmergencyContNo).MaximumLength(1024).WithName("Emergency Contact No");
            RuleFor(m => m.Reason).NotEmpty().WithName("Reason for Leave");
            RuleFor(c => c.Reason).MaximumLength(1024).WithName("Reason for Leave");
            RuleFor(c => c.RejectReason).MaximumLength(1024).WithName("Reject Reason");
        }
    }
    public class LeavesModel
    {
        public decimal Available { get; set; }
        public decimal Booked { get; set; }
        public string LeaveType { get; set; }
        public string Code { get; set; }
        public string LeaveTypeName { get; set; }
    }
}





