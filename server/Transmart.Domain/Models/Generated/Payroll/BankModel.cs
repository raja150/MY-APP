using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class BankModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string IFSCCode { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public int BankNoLength { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class BankModelValidator : AbstractValidator<BankModel>
    {
        public BankModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(32).WithName("Name");
            RuleFor(m => m.IFSCCode).NotEmpty().WithName("IFSC Code");
            RuleFor(c => c.IFSCCode).MinimumLength(11).WithName("IFSC Code");
            RuleFor(c => c.IFSCCode).MaximumLength(11).WithName("IFSC Code");
            RuleFor(m => m.DisplayName).NotEmpty().WithName("Display Name");
            RuleFor(c => c.DisplayName).MinimumLength(2).WithName("Display Name");
            RuleFor(c => c.DisplayName).MaximumLength(32).WithName("Display Name");
            RuleFor(m => m.AccountNo).NotEmpty().WithName("A/C No");
            RuleFor(c => c.AccountNo).MinimumLength(6).WithName("A/C No");
            RuleFor(c => c.AccountNo).MaximumLength(22).WithName("A/C No");
            RuleFor(c => c.BankNoLength).GreaterThanOrEqualTo(4).WithName("Bank Account No Of digits");
            RuleFor(c => c.BankNoLength).LessThanOrEqualTo(32).WithName("Bank Account No Of digits");
        }
    }
}
