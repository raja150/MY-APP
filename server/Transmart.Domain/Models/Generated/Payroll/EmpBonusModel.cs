using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EmpBonusModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public DateTime ReleasedOn { get; set; }
    }
    public class EmpBonusModelValidator : AbstractValidator<EmpBonusModel>
    {
        public EmpBonusModelValidator()
        {
            RuleFor(c => c.Amount).GreaterThanOrEqualTo(0).WithName("Bonus");
            RuleFor(c => c.Amount).LessThanOrEqualTo(999999).WithName("Bonus");
        }
    }
}
