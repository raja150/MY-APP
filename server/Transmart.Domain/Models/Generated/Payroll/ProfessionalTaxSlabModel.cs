using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class ProfessionalTaxSlabModel : BaseModel
    {
        public Guid ProfessionalTaxId { get; set; }
        [Required]
        public int From { get; set; }
        [Required]
        public int To { get; set; }
        [Required]
        public int Amount { get; set; }
    }
    public class ProfessionalTaxSlabModelValidator : AbstractValidator<ProfessionalTaxSlabModel>
    {
        public ProfessionalTaxSlabModelValidator()
        {
            RuleFor(c => c.From).GreaterThanOrEqualTo(0).WithName("From");
            RuleFor(c => c.From).LessThanOrEqualTo(999999).WithName("From");
            RuleFor(c => c.To).GreaterThanOrEqualTo(0).WithName("To");
            RuleFor(c => c.To).LessThanOrEqualTo(999999).WithName("To");
            RuleFor(c => c.Amount).GreaterThanOrEqualTo(0).WithName("Amount");
            RuleFor(c => c.Amount).LessThanOrEqualTo(999999).WithName("Amount");
        }
    }
}
