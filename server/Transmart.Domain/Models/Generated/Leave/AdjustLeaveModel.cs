using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class AdjustLeaveModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid LeaveTypeId { get; set; }
        [Required]
        public decimal NewBalance { get; set; }
        public string Reason { get; set; }
        [Required]
        public DateTime EffectiveFrom { get; set; }
        public DateTime? AddedOn { get; set; }
        public string Addedby { get; set; }
        [Required]
        public DateTime EffectiveTo { get; set; }
    }
    public class AdjustLeaveModelValidator : AbstractValidator<AdjustLeaveModel>
    {
        public AdjustLeaveModelValidator()
        {
            RuleFor(c => c.NewBalance).GreaterThanOrEqualTo(-999).WithName("New Balance");
            RuleFor(c => c.NewBalance).LessThanOrEqualTo(999).WithName("New Balance");
            RuleFor(c => c.Reason).MaximumLength(1024).WithName("Reason");
            RuleFor(c => c.Addedby).MaximumLength(1024).WithName("Addedby");
        }
    }
}
