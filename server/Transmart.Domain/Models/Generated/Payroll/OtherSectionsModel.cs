using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class OtherSectionsModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Limit { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class OtherSectionsModelValidator : AbstractValidator<OtherSectionsModel>
    {
        public OtherSectionsModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Section Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Section Name");
            RuleFor(c => c.Limit).GreaterThanOrEqualTo(0).WithName("Limit");
            RuleFor(c => c.Limit).LessThanOrEqualTo(999999).WithName("Limit");
        }
    }
}
