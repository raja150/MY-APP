using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class Section80DModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Limit { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class Section80DModelValidator : AbstractValidator<Section80DModel>
    {
        public Section80DModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(c => c.Limit).GreaterThanOrEqualTo(0).WithName("Limit");
            RuleFor(c => c.Limit).LessThanOrEqualTo(999999).WithName("Limit");
        }
    }
}
