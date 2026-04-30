using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class WorkHoursSettingModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int FullDayMinutes { get; set; }
        [Required]
        public int HalfDayMinutes { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class WorkHoursSettingModelValidator : AbstractValidator<WorkHoursSettingModel>
    {
        public WorkHoursSettingModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Display Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Display Name");
            RuleFor(c => c.Name).MaximumLength(32).WithName("Display Name");
            RuleFor(c => c.FullDayMinutes).GreaterThanOrEqualTo(0).WithName("Minimum Hours(in minutes) require to Consider as full Day");
            RuleFor(c => c.FullDayMinutes).LessThanOrEqualTo(1440).WithName("Minimum Hours(in minutes) require to Consider as full Day");
            RuleFor(c => c.HalfDayMinutes).GreaterThanOrEqualTo(0).WithName("Minimum Hours(in Minutes) require to Consider as half Day");
            RuleFor(c => c.HalfDayMinutes).LessThanOrEqualTo(1440).WithName("Minimum Hours(in Minutes) require to Consider as half Day");
        }
    }
}
