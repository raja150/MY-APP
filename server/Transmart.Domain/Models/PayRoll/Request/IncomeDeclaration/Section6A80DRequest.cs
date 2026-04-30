using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class Section6A80DRequest : BaseModel
    { 
        public Guid DeclarationId { get; set; } 
        public Guid Section80DId { get; set; } 
        public int Amount { get; set; } 
    }
    public class Section6A80DRequestValidator : AbstractValidator<Section6A80DRequest>
    {
        public Section6A80DRequestValidator()
        {
            RuleFor(m => m.Section80DId).NotEmpty().NotNull().When(x => x.Amount > 0).WithMessage("Section 80D is required");
            RuleFor(m => m.Amount).GreaterThan(0).When(x => x.Section80DId != Guid.Empty).WithMessage("Amount is required");
        }
    }
}
