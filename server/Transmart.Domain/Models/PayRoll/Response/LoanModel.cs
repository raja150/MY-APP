using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TranSmart.Domain.Models.Payroll
{
    public partial class  LoanModel:BaseModel
    {

    }
    public  class LoanValidator : AbstractValidator<LoanModel>
    {

        public LoanValidator()
        {
            RuleFor(x => x.MonthlyAmount).LessThanOrEqualTo(x => x.LoanAmount).WithMessage(Resource.Monthly_amount_should_be_less_than_loan_Amount);
            RuleFor(x => x.DeductFrom).GreaterThanOrEqualTo(x => x.LoanReleasedOn).WithMessage(Resource.Loan_deducation_date_should_be_greaterthan_or_equal_to_the_loan_release_date);
        }
    }
    
   
}
