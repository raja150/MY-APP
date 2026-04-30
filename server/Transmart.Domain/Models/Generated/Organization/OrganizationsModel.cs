using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class OrganizationsModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string logo { get; set; }
        [Required]
        public int DateShowFormat { get; set; }
        [Required]
        public string PAN { get; set; }
        [Required]
        public string TAN { get; set; }
        [Required]
        public int ProbationPeriodType { get; set; }
        [Required]
        public int MonthStartDay { get; set; }
        public string AddressOne { get; set; }
        public string AddressSecond { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class OrganizationsModelValidator : AbstractValidator<OrganizationsModel>
    {
        public OrganizationsModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(c => c.logo).MaximumLength(1024).WithName("Logo");
            RuleFor(m => m.PAN).NotEmpty().WithName("PAN");
            RuleFor(m => m.PAN).Matches(Resource.RegEx_PAN).WithName("PAN");
            RuleFor(c => c.PAN).MaximumLength(1024).WithName("PAN");
            RuleFor(m => m.TAN).NotEmpty().WithName("TAN");
            RuleFor(c => c.TAN).MaximumLength(1024).WithName("TAN");
            RuleFor(c => c.MonthStartDay).GreaterThanOrEqualTo(1).WithName("Payroll Month Start Day");
            RuleFor(c => c.MonthStartDay).LessThanOrEqualTo(31).WithName("Payroll Month Start Day");
            RuleFor(c => c.AddressOne).MaximumLength(1024).WithName("Address 1");
            RuleFor(c => c.AddressSecond).MaximumLength(1024).WithName("Address 2");
            RuleFor(c => c.City).MaximumLength(1024).WithName("City");
            RuleFor(c => c.State).MaximumLength(1024).WithName("State");
            RuleFor(c => c.Pincode).MaximumLength(1024).WithName("Pin Code");
        }
    }
}
