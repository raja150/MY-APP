using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class Section6AOtherRequest : BaseModel
    { 
        public Guid DeclarationId { get; set; } 
        public Guid OtherSectionsId { get; set; } 
        public int Amount { get; set; } 
    }
    public class SectionOtherRequestValidator : AbstractValidator<Section6AOtherRequest>
    {
        public SectionOtherRequestValidator()
        {
            RuleFor(m => m.OtherSectionsId).NotEmpty().NotNull().When(x =>x.Amount > 0).WithMessage("Other Section is required");
            RuleFor(m => m.Amount).GreaterThan(0).When(x => x.OtherSectionsId != Guid.Empty).WithMessage("Amount is required");
        }
    }

}
