using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class FunctionalAreaModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class FunctionalAreaModelValidator : AbstractValidator<FunctionalAreaModel>
    {
        public FunctionalAreaModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(128).WithName("Name");
        }
    }
}
