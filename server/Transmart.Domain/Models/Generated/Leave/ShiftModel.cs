using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ShiftModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int StartFrom { get; set; }
        [Required]
        public int EndsOn { get; set; }
        public int? loginGraceTime { get; set; }
        public int? logoutGraceTime { get; set; }
        public int? Allowance { get; set; }
        [Required]
        public int BreakTime { get; set; }
        public string Desciption { get; set; }
        [Required]
        public int NoOfBreaks { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class ShiftModelValidator : AbstractValidator<ShiftModel>
    {
        public ShiftModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(c => c.StartFrom).GreaterThanOrEqualTo(0).WithName("Starts From(Min)");
            RuleFor(c => c.StartFrom).LessThanOrEqualTo(1440).WithName("Starts From(Min)");
            RuleFor(c => c.EndsOn).GreaterThanOrEqualTo(0).WithName("Ends On(Min)");
            RuleFor(c => c.EndsOn).LessThanOrEqualTo(1440).WithName("Ends On(Min)");
            RuleFor(c => c.loginGraceTime).GreaterThanOrEqualTo(0).WithName("login Grace Period(Min)");
            RuleFor(c => c.loginGraceTime).LessThanOrEqualTo(90).WithName("login Grace Period(Min)");
            RuleFor(c => c.logoutGraceTime).GreaterThanOrEqualTo(0).WithName("logout Grace Period(Min)");
            RuleFor(c => c.logoutGraceTime).LessThanOrEqualTo(90).WithName("logout Grace Period(Min)");
            RuleFor(c => c.BreakTime).GreaterThanOrEqualTo(0).WithName("Break Time");
            RuleFor(c => c.BreakTime).LessThanOrEqualTo(999).WithName("Break Time");
            RuleFor(c => c.Desciption).MaximumLength(1024).WithName("Desciption");
            RuleFor(c => c.NoOfBreaks).GreaterThanOrEqualTo(0).WithName("No Of Breaks");
            RuleFor(c => c.NoOfBreaks).LessThanOrEqualTo(9).WithName("No Of Breaks");
        }
    }
}
