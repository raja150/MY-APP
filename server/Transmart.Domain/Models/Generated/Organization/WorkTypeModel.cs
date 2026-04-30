using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class WorkTypeModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool SalaryPaying { get; set; }
        [Required]
        public bool CalculateAtt { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class WorkTypeModelValidator : AbstractValidator<WorkTypeModel>
    {
        public WorkTypeModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(32).WithName("Name");
        }
    }
}
