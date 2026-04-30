using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class Section80CModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class Section80CModelValidator : AbstractValidator<Section80CModel>
    {
        public Section80CModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
