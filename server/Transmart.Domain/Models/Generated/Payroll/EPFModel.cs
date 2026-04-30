using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EPFModel : BaseModel
    {
        public Guid PaySettingsId { get; set; }
        [Required]
        public int EnableEPF { get; set; }
        public int? EmployeeContrib { get; set; }
        public bool IncludeinCTC { get; set; }
        public string PFConfiguration { get; set; }
        public int? RestrictedWage { get; set; }
    }
    public class EPFModelValidator : AbstractValidator<EPFModel>
    {
        public EPFModelValidator()
        {
            RuleFor(c => c.PFConfiguration).MaximumLength(1024).WithName("PF Configuration when LOP Applied");
            RuleFor(c => c.RestrictedWage).GreaterThanOrEqualTo(0).WithName("Restricted PF Wage");
            RuleFor(c => c.RestrictedWage).LessThanOrEqualTo(99999).WithName("Restricted PF Wage");
        }
    }
}
