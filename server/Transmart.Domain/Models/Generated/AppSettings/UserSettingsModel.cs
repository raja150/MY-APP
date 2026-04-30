using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.AppSettings
{
    public partial class UserSettingsModel : BaseModel
    {
        [Required]
        public int PasswordExpiry { get; set; }
        [Required]
        public int MinimumPassword { get; set; }
        [Required]
        public int MaximumPassword { get; set; }
        public string AllowedSpecial { get; set; }
        public string TrackLoginInfo { get; set; }
        [Required]
        public int AllowNumber { get; set; }
        public string DisableUserAc { get; set; }
    }
    public class UserSettingsModelValidator : AbstractValidator<UserSettingsModel>
    {
        public UserSettingsModelValidator()
        {
            RuleFor(c => c.PasswordExpiry).GreaterThanOrEqualTo(7).WithName("Password Expiry Days");
            RuleFor(c => c.PasswordExpiry).LessThanOrEqualTo(90).WithName("Password Expiry Days");
            RuleFor(c => c.MinimumPassword).GreaterThanOrEqualTo(4).WithName("Minimum Password Length");
            RuleFor(c => c.MinimumPassword).LessThanOrEqualTo(12).WithName("Minimum Password Length");
            RuleFor(c => c.MaximumPassword).GreaterThanOrEqualTo(4).WithName("Maximum Password Length");
            RuleFor(c => c.MaximumPassword).LessThanOrEqualTo(12).WithName("Maximum Password Length");
            RuleFor(c => c.AllowedSpecial).MaximumLength(1024).WithName("Allowed Special Characters In Password");
            RuleFor(c => c.TrackLoginInfo).MaximumLength(1024).WithName("Track Login Information");
            RuleFor(c => c.AllowNumber).GreaterThanOrEqualTo(3).WithName("Allow Number of Invalid Login Attempts");
            RuleFor(c => c.AllowNumber).LessThanOrEqualTo(12).WithName("Allow Number of Invalid Login Attempts");
            RuleFor(c => c.DisableUserAc).MaximumLength(1024).WithName("Disable User Account on Reaching The Max No. of Invalid Attempts");
        }
    }
}
