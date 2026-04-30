using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class PaySetupModel : BaseModel
    {
        public Guid PaySettingsId { get; set; }
        [Required]
        public int SalaryDaysType { get; set; }
        [Required]
        public int PayOn { get; set; }
        public int? MonthDay { get; set; }
    }
    public class PaySetupModelValidator : AbstractValidator<PaySetupModel>
    {
        public PaySetupModelValidator()
        {
            RuleFor(c => c.MonthDay).GreaterThanOrEqualTo(1).WithName("Month Day");
            RuleFor(c => c.MonthDay).LessThanOrEqualTo(31).WithName("Month Day");
        }
    }
}
