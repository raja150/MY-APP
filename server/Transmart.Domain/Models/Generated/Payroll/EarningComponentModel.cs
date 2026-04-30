using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EarningComponentModel : BaseModel
    {
        [Required]
        public int EarningType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool PartOfSalary { get; set; }
        [Required]
        public bool ProrataBasis { get; set; }
        [Required]
        public int EPFContribution { get; set; }
        [Required]
        public bool ESIContribution { get; set; }
        [Required]
        public bool ShowInPayslip { get; set; }
        [Required]
        public bool PartEmployerCTC { get; set; }
        [Required]
        public bool HideWhenZero { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class EarningComponentModelValidator : AbstractValidator<EarningComponentModel>
    {
        public EarningComponentModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
