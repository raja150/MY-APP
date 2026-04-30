using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class DeductionComponentModel : BaseModel
    {
        [Required]
        public int Deduct { get; set; }
        [Required]
        public string Name { get; set; }
        public int? DeductionPlan { get; set; }
        public bool ProrataBasis { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class DeductionComponentModelValidator : AbstractValidator<DeductionComponentModel>
    {
        public DeductionComponentModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
