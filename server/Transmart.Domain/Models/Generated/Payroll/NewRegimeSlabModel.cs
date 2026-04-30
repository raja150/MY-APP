using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class NewRegimeSlabModel : BaseModel
    {
        public Guid PaySettingsId { get; set; }
        [Required]
        public int IncomeFrom { get; set; }
        [Required]
        public int IncomeTo { get; set; }
        [Required]
        public int TaxRate { get; set; }
    }
    public class NewRegimeSlabModelValidator : AbstractValidator<NewRegimeSlabModel>
    {
        public NewRegimeSlabModelValidator()
        {
            RuleFor(c => c.IncomeFrom).GreaterThanOrEqualTo(0).WithName("Annual Income From");
            RuleFor(c => c.IncomeFrom).LessThanOrEqualTo(9999999).WithName("Annual Income From");
            RuleFor(c => c.IncomeTo).GreaterThanOrEqualTo(0).WithName("Annual Income To");
            RuleFor(c => c.IncomeTo).LessThanOrEqualTo(99999999).WithName("Annual Income To");
            RuleFor(c => c.TaxRate).GreaterThanOrEqualTo(0).WithName("Tax Rate");
            RuleFor(c => c.TaxRate).LessThanOrEqualTo(100).WithName("Tax Rate");
        }
    }
}
