using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class HomeLoanPayRequest : BaseModel
    {
        public string No { get; set; }
        public Guid DeclarationId { get; set; }
        public decimal InterestPaid { get; set; }
        public string NameOfLender { get; set; }
        public string LenderPAN { get; set; }
        public decimal Principle { get; set; }

    }
    public class HomeLoanPayRequestValidator : AbstractValidator<HomeLoanPayRequest>
    {
        public HomeLoanPayRequestValidator()
        {
            RuleFor(m => m.InterestPaid).GreaterThanOrEqualTo(1).When(x => x.InterestPaid > 0).WithMessage("Interest Paid is Required.");
            RuleFor(m => m.Principle).GreaterThanOrEqualTo(1).When(x => x.InterestPaid > 0).WithMessage("Home Loan is Required.");
            RuleFor(m => m.NameOfLender).NotNull().NotEmpty().When(x => x.InterestPaid > 0 || x.Principle > 0).WithMessage("Name Of Lender is Required.");
            RuleFor(m => m.LenderPAN).NotNull().NotEmpty().When(x => x.InterestPaid > 0 || x.Principle > 0).WithMessage("Lender PAN is Required.");
        }
    }

}
