using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeePermanentAdModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string AddressLineOne { get; set; }
        [Required]
        public int SameAsPresent { get; set; }
        public string AddressLineTwo { get; set; }
        public string CityOrTown { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
    public class EmployeePermanentAdModelValidator : AbstractValidator<EmployeePermanentAdModel>
    {
        public EmployeePermanentAdModelValidator()
        {
            RuleFor(c => c.AddressLineOne).MaximumLength(1024).WithName("Address Line 1");
            RuleFor(c => c.AddressLineTwo).MaximumLength(1024).WithName("Address Line 2");
            RuleFor(c => c.CityOrTown).MaximumLength(1024).WithName("City / town");
            RuleFor(c => c.State).MaximumLength(1024).WithName("State");
            RuleFor(c => c.Country).MaximumLength(1024).WithName("Country");
        }
    }
}
