using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class ProbationPeriodModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ProbationPeriodType { get; set; }
        [Required]
        public int ProbationPeriodTime { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class ProbationPeriodModelValidator : AbstractValidator<ProbationPeriodModel>
    {
        public ProbationPeriodModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(32).WithName("Name");
            RuleFor(c => c.ProbationPeriodTime).GreaterThanOrEqualTo(1).WithName("Probation Period Days/Months");
            RuleFor(c => c.ProbationPeriodTime).LessThanOrEqualTo(999).WithName("Probation Period Days/Months");
        }
    }
}
