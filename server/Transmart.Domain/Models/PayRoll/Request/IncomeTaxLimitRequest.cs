using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Request
{
    public class IncomeTaxLimitRequest : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? PayMonthId { get; set; }
        public int Amount { get; set; }
    }
    public class IncomeTaxLimitModelValidator : AbstractValidator<IncomeTaxLimitRequest>
    {
        public IncomeTaxLimitModelValidator()
        {
            RuleFor(m => m.EmployeeId).NotEmpty().WithName("EmployeeNo ").WithMessage("Employee ID is required");
            RuleFor(m => m.Amount).GreaterThanOrEqualTo(0).WithName("Amount").WithMessage("Grater than or equal to 0");
            RuleFor(c => c.PayMonthId).NotNull().When(x => x.ID == Guid.Empty).WithMessage("PayMonth are required");
        }
    }
}