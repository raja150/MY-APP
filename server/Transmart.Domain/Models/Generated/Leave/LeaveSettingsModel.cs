using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class LeaveSettingsModel : BaseModel
    {
        [Required]
        public int HourCalculation { get; set; }
        public int? FullDayHours { get; set; }
        public int? HalfDayHours { get; set; }
        public int? IncludeWeekend { get; set; }
        public int? IncludeHoliday { get; set; }
        public int? IncludeLeave { get; set; }
        public Guid? CompCreditToId { get; set; }
        public Guid CompoLeaveTypeId { get; set; }
        [Required]
        public int ExpireInMonths { get; set; }
        public int? WeekendPeriod { get; set; }
        public bool WeekendPeriodDn { get; set; }
        public int? HolidayPeriod { get; set; }
        public bool HolidayPeriodDn { get; set; }
    }
    public class LeaveSettingsModelValidator : AbstractValidator<LeaveSettingsModel>
    {
        public LeaveSettingsModelValidator()
        {
            RuleFor(c => c.FullDayHours).GreaterThanOrEqualTo(0).WithName("Minimum Hours require to Consider as full Day");
            RuleFor(c => c.FullDayHours).LessThanOrEqualTo(24).WithName("Minimum Hours require to Consider as full Day");
            RuleFor(c => c.HalfDayHours).GreaterThanOrEqualTo(0).WithName("Minimum Hours require to Consider as half Day");
            RuleFor(c => c.HalfDayHours).LessThanOrEqualTo(12).WithName("Minimum Hours require to Consider as half Day");
            RuleFor(c => c.ExpireInMonths).GreaterThanOrEqualTo(0).WithName("Expire In Months");
            RuleFor(c => c.ExpireInMonths).LessThanOrEqualTo(99).WithName("Expire In Months");
            RuleFor(c => c.WeekendPeriod).GreaterThanOrEqualTo(0).WithName("Weekends Between Leave Period Count as leave after days");
            RuleFor(c => c.WeekendPeriod).LessThanOrEqualTo(99).WithName("Weekends Between Leave Period Count as leave after days");
            RuleFor(c => c.HolidayPeriod).GreaterThanOrEqualTo(0).WithName("Holidays Between Leave Period Count as leave after days");
            RuleFor(c => c.HolidayPeriod).LessThanOrEqualTo(99).WithName("Holidays Between Leave Period Count as leave after days");
        }
    }
}
