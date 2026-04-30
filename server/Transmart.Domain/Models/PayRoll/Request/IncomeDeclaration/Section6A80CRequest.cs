using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class Section6A80CRequest : BaseModel
    {
        public Guid DeclarationId { get; set; } 
        public Guid Section80CId { get; set; } 
        public int Amount { get; set; } 
    }
    public class Section6A80CRequestValidator : AbstractValidator<Section6A80CRequest>
    {
        public Section6A80CRequestValidator()
        {
            RuleFor(m => m.Section80CId).NotEmpty().NotNull().When(x => x.Amount > 0).WithMessage("Section 80C is required");
            RuleFor(m => m.Amount).GreaterThan(0).When(x => x.Section80CId != Guid.Empty).WithMessage("Amount is required");
        }
    }
}
