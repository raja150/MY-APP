using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeDeviceModel : BaseModel
    {
        [Required]
        public int EmployeeId { get; set; }
        public string MobileNumber { get; set; }
        [Required]
        public int ComputerType { get; set; }
        [Required]
        public string HostName { get; set; }
        public bool IsActZeroInstalled { get; set; }
        public bool IsKInstalled { get; set; }
        [Required]
        public DateTime InstalledOn { get; set; }
        public bool IsUninstalled { get; set; }
        public DateTime? UninstalledOn { get; set; }
    }
    public class EmployeeDeviceModelValidator : AbstractValidator<EmployeeDeviceModel>
    {
        public EmployeeDeviceModelValidator()
        {
            RuleFor(c => c.MobileNumber).MaximumLength(1024).WithName("Mobile Number");
            RuleFor(m => m.HostName).NotEmpty().WithName("Host Name");
            RuleFor(c => c.HostName).MaximumLength(1024).WithName("Host Name");
        }
    }
}
