using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Request
{
    public class ArrearsRequest : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? PayMonthId { get; set; }
        public int Pay { get; set; }
    }
    public class ArrearsModelValidator : AbstractValidator<ArrearsRequest>
    {
        public ArrearsModelValidator()
        {
            RuleFor(m => m.EmployeeId).NotEmpty().WithName("EmployeeNo ").WithMessage("Employee No is required");
            RuleFor(m => m.Pay).NotEmpty().WithName("Pay");
            RuleFor(c => c.PayMonthId).NotNull().When(x => x.ID == Guid.Empty).WithMessage("PayMonth are required");
        }
    }
}
