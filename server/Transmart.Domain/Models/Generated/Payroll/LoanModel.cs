using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class LoanModel : BaseModel
    {
        public string LoanNo { get; set; }
        public Guid EmployeeId { get; set; }
        [Required]
        public DateTime LoanReleasedOn { get; set; }
        [Required]
        public int LoanAmount { get; set; }
        [Required]
        public DateTime DeductFrom { get; set; }
        [Required]
        public int MonthlyAmount { get; set; }
        [Required]
        public string Notes { get; set; }
        public int? Due { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class LoanModelValidator : AbstractValidator<LoanModel>
    {
        public LoanModelValidator()
        {
            RuleFor(c => c.LoanNo).MaximumLength(1024).WithName("Loan Number");
            RuleFor(c => c.LoanAmount).GreaterThanOrEqualTo(0).WithName("Amount");
            RuleFor(c => c.LoanAmount).LessThanOrEqualTo(999999).WithName("Amount");
            RuleFor(c => c.MonthlyAmount).GreaterThanOrEqualTo(0).WithName("Monthly Deduction Amount");
            RuleFor(c => c.MonthlyAmount).LessThanOrEqualTo(999999).WithName("Monthly Deduction Amount");
            RuleFor(m => m.Notes).NotEmpty().WithName("Notes");
            RuleFor(c => c.Notes).MaximumLength(1024).WithName("Notes");
        }
    }
}
