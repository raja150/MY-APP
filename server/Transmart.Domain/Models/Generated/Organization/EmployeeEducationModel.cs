using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeEducationModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public string Medium { get; set; }
        [Required]
        public string Institute { get; set; }
        [Required]
        public decimal Percentage { get; set; }
        [Required]
        public int YearOfPassing { get; set; }
    }
    public class EmployeeEducationModelValidator : AbstractValidator<EmployeeEducationModel>
    {
        public EmployeeEducationModelValidator()
        {
            RuleFor(m => m.Qualification).NotEmpty().WithName("Qualification");
            RuleFor(c => c.Qualification).MinimumLength(2).WithName("Qualification");
            RuleFor(c => c.Qualification).MaximumLength(64).WithName("Qualification");
            RuleFor(m => m.Degree).NotEmpty().WithName("Degree");
            RuleFor(c => c.Degree).MinimumLength(2).WithName("Degree");
            RuleFor(c => c.Degree).MaximumLength(64).WithName("Degree");
            RuleFor(m => m.Medium).NotEmpty().WithName("Medium");
            RuleFor(c => c.Medium).MinimumLength(2).WithName("Medium");
            RuleFor(c => c.Medium).MaximumLength(32).WithName("Medium");
            RuleFor(m => m.Institute).NotEmpty().WithName("Institute");
            RuleFor(c => c.Institute).MinimumLength(2).WithName("Institute");
            RuleFor(c => c.Institute).MaximumLength(1264).WithName("Institute");
            RuleFor(c => c.Percentage).GreaterThanOrEqualTo(1).WithName("Percentage");
            RuleFor(c => c.Percentage).LessThanOrEqualTo(100).WithName("Percentage");
            RuleFor(c => c.YearOfPassing).GreaterThanOrEqualTo(1900).WithName("Year of passing");
            RuleFor(c => c.YearOfPassing).LessThanOrEqualTo(2030).WithName("Year of passing");
        }
    }
}
