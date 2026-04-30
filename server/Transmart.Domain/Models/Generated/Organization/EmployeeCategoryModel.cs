using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeCategoryModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class EmployeeCategoryModelValidator : AbstractValidator<EmployeeCategoryModel>
    {
        public EmployeeCategoryModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
