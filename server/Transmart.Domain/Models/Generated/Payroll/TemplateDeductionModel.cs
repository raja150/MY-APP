using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class TemplateDeductionModel : BaseModel
    {
        public Guid TemplateId { get; set; }
        public Guid ComponentId { get; set; }
        [Required]
        public int Amount { get; set; }
    }
    public class TemplateDeductionModelValidator : AbstractValidator<TemplateDeductionModel>
    {
        public TemplateDeductionModelValidator()
        {
            RuleFor(c => c.Amount).GreaterThanOrEqualTo(0).WithName("Amount");
            RuleFor(c => c.Amount).LessThanOrEqualTo(999999).WithName("Amount");
        }
    }
}
