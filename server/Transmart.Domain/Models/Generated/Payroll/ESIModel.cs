using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class ESIModel : BaseModel
    {
        public Guid PaySettingsId { get; set; }
        [Required]
        public int EnableESI { get; set; }
        public int? ESISalaryLimit { get; set; }
        public string ESINo { get; set; }
        public decimal? EmployeesCont { get; set; }
        public decimal? EmployerCont { get; set; }
        public bool AddToCTC { get; set; }
    }
    public class ESIModelValidator : AbstractValidator<ESIModel>
    {
        public ESIModelValidator()
        {
            RuleFor(c => c.ESISalaryLimit).GreaterThanOrEqualTo(0).WithName("ESI Salary Limit");
            RuleFor(c => c.ESISalaryLimit).LessThanOrEqualTo(99999).WithName("ESI Salary Limit");
            RuleFor(c => c.ESINo).MaximumLength(1024).WithName("ESI No");
            RuleFor(c => c.EmployeesCont).GreaterThanOrEqualTo(0).WithName("Employees'' Contribution % of Gross Pay");
            RuleFor(c => c.EmployeesCont).LessThanOrEqualTo(100).WithName("Employees'' Contribution % of Gross Pay");
            RuleFor(c => c.EmployerCont).GreaterThanOrEqualTo(0).WithName("Employer''s Contribution % of Gross Pay");
            RuleFor(c => c.EmployerCont).LessThanOrEqualTo(100).WithName("Employer''s Contribution % of Gross Pay");
        }
    }
}
