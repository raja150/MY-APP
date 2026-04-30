using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class DeclarationSettingModel : BaseModel
    {
        public Guid PaySettingsId { get; set; }
        [Required]
        public int Lock { get; set; }
        [Required]
        public int MaxLimitEightyC { get; set; }
        [Required]
        public int MaxLimitEightyD { get; set; }
        [Required]
        public decimal EducationCess { get; set; }
        [Required]
        public int HouseLoanInt { get; set; }
        [Required]
        public int TaxDedLastMonth { get; set; }
    }
    public class DeclarationSettingModelValidator : AbstractValidator<DeclarationSettingModel>
    {
        public DeclarationSettingModelValidator()
        {
            RuleFor(c => c.MaxLimitEightyC).GreaterThanOrEqualTo(0).WithName("Max limit 80C");
            RuleFor(c => c.MaxLimitEightyC).LessThanOrEqualTo(999999).WithName("Max limit 80C");
            RuleFor(c => c.MaxLimitEightyD).GreaterThanOrEqualTo(0).WithName("Max Limit 80D");
            RuleFor(c => c.MaxLimitEightyD).LessThanOrEqualTo(999999).WithName("Max Limit 80D");
            RuleFor(c => c.EducationCess).GreaterThanOrEqualTo(0).WithName("Education Cess in %");
            RuleFor(c => c.EducationCess).LessThanOrEqualTo(100).WithName("Education Cess in %");
            RuleFor(c => c.HouseLoanInt).GreaterThanOrEqualTo(0).WithName("House Loan Interest limit");
            RuleFor(c => c.HouseLoanInt).LessThanOrEqualTo(999999).WithName("House Loan Interest limit");
        }
    }
}
