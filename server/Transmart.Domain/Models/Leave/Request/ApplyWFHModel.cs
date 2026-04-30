using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ApplyWfhModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public Guid? ApprovedById { get; set; }
        [Required]
        public DateTime FromDateC { get; set; }
        [Required]
        public DateTime ToDateC { get; set; }
		public bool? FromHalf { get; set; }
		public bool? ToHalf { get; set; }
		[Required]
        public string ReasonForWFH { get; set; }
        public string AdminReason { get; set; }
        public int? Status { get; set; }
        public string EmployeeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string RejectReason { get; set; }
		public decimal NoOfDays { get; set; }
    }

    public class ApplyWfhModelValidator : AbstractValidator<ApplyWfhModel>
    {
        public ApplyWfhModelValidator()
        {
            RuleFor(m => m.ReasonForWFH).NotEmpty().WithName("Reason For work from Home");
            RuleFor(c => c.AdminReason).MaximumLength(1024).WithName("Textarea");
        }
    }
}
