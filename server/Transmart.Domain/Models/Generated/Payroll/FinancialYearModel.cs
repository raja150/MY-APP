using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class FinancialYearModel : BaseModel
    {
        public Guid PaySettingsId { get; set; }
        [Required]
        public int FromYear { get; set; }
        [Required]
        public int ToYear { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Closed { get; set; }
    }
    public class FinancialYearModelValidator : AbstractValidator<FinancialYearModel>
    {
        public FinancialYearModelValidator()
        {
            RuleFor(c => c.FromYear).GreaterThanOrEqualTo(2020).WithName("From Year");
            RuleFor(c => c.FromYear).LessThanOrEqualTo(2030).WithName("From Year");
            RuleFor(c => c.ToYear).GreaterThanOrEqualTo(2020).WithName("To Year");
            RuleFor(c => c.ToYear).LessThanOrEqualTo(2030).WithName("To Year");
            RuleFor(m => m.Name).NotEmpty().WithName("Display Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Display Name");
        }
    }
}
