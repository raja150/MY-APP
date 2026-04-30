using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeEmergencyAdModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string AddressLineOne { get; set; }
        [Required]
        public string AddressLineTwo { get; set; }
        [Required]
        public string CityOrTown { get; set; }
        [Required]
        public string EmergencyConNo { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
    }
    public class EmployeeEmergencyAdModelValidator : AbstractValidator<EmployeeEmergencyAdModel>
    {
        public EmployeeEmergencyAdModelValidator()
        {
            RuleFor(m => m.AddressLineOne).NotEmpty().WithName("Address Line 1");
            RuleFor(c => c.AddressLineOne).MaximumLength(1024).WithName("Address Line 1");
            RuleFor(m => m.AddressLineTwo).NotEmpty().WithName("Address Line 2");
            RuleFor(c => c.AddressLineTwo).MaximumLength(1024).WithName("Address Line 2");
            RuleFor(m => m.CityOrTown).NotEmpty().WithName("City / town");
            RuleFor(c => c.CityOrTown).MaximumLength(1024).WithName("City / town");
            RuleFor(m => m.EmergencyConNo).NotEmpty().WithName("Emergency Contact No");
            RuleFor(c => c.EmergencyConNo).MaximumLength(1024).WithName("Emergency Contact No");
            RuleFor(m => m.State).NotEmpty().WithName("State");
            RuleFor(c => c.State).MinimumLength(2).WithName("State");
            RuleFor(c => c.State).MaximumLength(64).WithName("State");
            RuleFor(m => m.Country).NotEmpty().WithName("Country");
            RuleFor(c => c.Country).MinimumLength(2).WithName("Country");
            RuleFor(c => c.Country).MaximumLength(64).WithName("Country");
        }
    }
}
