using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class ApplyCompensatoryWorkingDayModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? ApprovedById { get; set; }
        public Guid? ShiftId { get; set; }
        public string EmailID { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public string ReasonForApply { get; set; }
        public string AdminReason { get; set; }
    }
    public class ApplyCompensatoryWorkingDayModelValidator : AbstractValidator<ApplyCompensatoryWorkingDayModel>
    {
        public ApplyCompensatoryWorkingDayModelValidator()
        {
            RuleFor(c => c.EmailID).MaximumLength(1024).WithName("Email ID");
            RuleFor(m => m.ReasonForApply).NotEmpty().WithName("Reason For Applying Compensatory Working  Day");
            RuleFor(c => c.ReasonForApply).MaximumLength(1024).WithName("Reason For Applying Compensatory Working  Day");
            RuleFor(c => c.AdminReason).MaximumLength(1024).WithName("Textarea");
        }
    }

}
