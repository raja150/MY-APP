using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class PaySettingsModel : BaseModel
    {
        public Guid OrganizationId { get; set; }
        [Required]
        public string TaxDeductor { get; set; }
        [Required]
        public string TaxDeductorFNam { get; set; }
        [Required]
        public int FYFromMonth { get; set; }
        [Required]
        public int FYFromDay { get; set; }
    }
    public class PaySettingsModelValidator : AbstractValidator<PaySettingsModel>
    {
        public PaySettingsModelValidator()
        {
            RuleFor(m => m.TaxDeductor).NotEmpty().WithName("Tax Deductor''s Name");
            RuleFor(c => c.TaxDeductor).MaximumLength(1024).WithName("Tax Deductor''s Name");
            RuleFor(m => m.TaxDeductorFNam).NotEmpty().WithName("Tax Deductor''s Father Name");
            RuleFor(c => c.TaxDeductorFNam).MaximumLength(1024).WithName("Tax Deductor''s Father Name");
            RuleFor(c => c.FYFromDay).GreaterThanOrEqualTo(0).WithName("Financial Start Day in Month");
            RuleFor(c => c.FYFromDay).LessThanOrEqualTo(31).WithName("Financial Start Day in Month");
        }
    }
}
