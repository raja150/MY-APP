using FluentValidation;
using System;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class HraDeclarationRequest : BaseModel
    {
        public string No { get; set; }
        public Guid DeclarationId { get; set; }
        public DateTime? RentalFrom { get; set; }
        public DateTime? RentalTo { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pan { get; set; }
        public string Landlord { get; set; }
    }
    public class HraDeclarationRequestValidator : AbstractValidator<HraDeclarationRequest>
    {
        public HraDeclarationRequestValidator()
        {
            RuleFor(m => m.RentalFrom).NotEmpty().NotNull().When(x => x.Amount > 0).WithMessage("Rental From is required.");
            RuleFor(m => m.RentalTo).NotEmpty().NotNull().When(x => x.Amount > 0).WithMessage("Rental To is required."); 
            RuleFor(m => m.Address).NotEmpty().NotNull().When(x => x.Amount > 0).WithMessage("Amount is required.");
            RuleFor(m => m.City).NotEmpty().NotNull().When(x => x.Amount > 0).WithMessage("City is required.");
            RuleFor(m => m.Pan).NotEmpty().NotNull().When(x => x.Amount >= 100000).WithMessage("PAN is required.");
            RuleFor(m => m.Landlord).NotEmpty().NotNull().When(x => x.Amount >= 100000).WithMessage("Landlord name is required.");
        }
    }
}
