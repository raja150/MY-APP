using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class TemplateEarningModel : BaseModel
    {
        public Guid TemplateId { get; set; }
        public Guid ComponentId { get; set; }
        [Required]
        public int Type { get; set; }
        public int? Percentage { get; set; }
        public int? Amount { get; set; }
        public int? PercentOn { get; set; }
        public Guid? PercentOnCompId { get; set; }
    }
    public class TemplateEarningModelValidator : AbstractValidator<TemplateEarningModel>
    {
        public TemplateEarningModelValidator()
        {
            RuleFor(c => c.Percentage).GreaterThanOrEqualTo(0).WithName("Percentage");
            RuleFor(c => c.Percentage).LessThanOrEqualTo(100).WithName("Percentage");
            RuleFor(c => c.Amount).GreaterThanOrEqualTo(0).WithName("Amount");
            RuleFor(c => c.Amount).LessThanOrEqualTo(999999).WithName("Amount");
        }
    }
}
