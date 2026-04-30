using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeePresentAdModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string AddressLineOne { get; set; }
        [Required]
        public string AddressLineTwo { get; set; }
        [Required]
        public string CityOrTown { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
    }
    public class EmployeePresentAdModelValidator : AbstractValidator<EmployeePresentAdModel>
    {
        public EmployeePresentAdModelValidator()
        {
            RuleFor(m => m.AddressLineOne).NotEmpty().WithName("Address Line 1");
            RuleFor(c => c.AddressLineOne).MaximumLength(1024).WithName("Address Line 1");
            RuleFor(m => m.AddressLineTwo).NotEmpty().WithName("Address Line 2");
            RuleFor(c => c.AddressLineTwo).MaximumLength(1024).WithName("Address Line 2");
            RuleFor(m => m.CityOrTown).NotEmpty().WithName("City / town");
            RuleFor(c => c.CityOrTown).MaximumLength(1024).WithName("City / town");
            RuleFor(m => m.State).NotEmpty().WithName("State");
            RuleFor(c => c.State).MinimumLength(2).WithName("State");
            RuleFor(c => c.State).MaximumLength(64).WithName("State");
            RuleFor(m => m.Country).NotEmpty().WithName("Country");
            RuleFor(c => c.Country).MinimumLength(2).WithName("Country");
            RuleFor(c => c.Country).MaximumLength(64).WithName("Country");
        }
    }
}
