using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class WeekOffDaysModel : BaseModel
    {
        public Guid WeekOffSetupId { get; set; }
        [Required]
        public int Type { get; set; }
        public int? WeekDay { get; set; }
        public string WeekNoInMonth { get; set; }
        public int? WeekInYear { get; set; }
    }
    public class WeekOffDaysModelValidator : AbstractValidator<WeekOffDaysModel>
    {
        public WeekOffDaysModelValidator()
        {
            RuleFor(c => c.WeekNoInMonth).MaximumLength(1024).WithName("Week number in month");
        }
    }
}
