using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeWorkExpModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string Organization { get; set; }
        [Required]
        public string Designation { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [Required]
        public int Salary { get; set; }
        public string ResignedReason { get; set; }
    }
    public class EmployeeWorkExpModelValidator : AbstractValidator<EmployeeWorkExpModel>
    {
        public EmployeeWorkExpModelValidator()
        {
            RuleFor(m => m.Organization).NotEmpty().WithName("Organization");
            RuleFor(c => c.Organization).MinimumLength(2).WithName("Organization");
            RuleFor(c => c.Organization).MaximumLength(128).WithName("Organization");
            RuleFor(m => m.Designation).NotEmpty().WithName("Designation");
            RuleFor(c => c.Designation).MinimumLength(2).WithName("Designation");
            RuleFor(c => c.Designation).MaximumLength(128).WithName("Designation");
            RuleFor(c => c.Salary).GreaterThanOrEqualTo(999).WithName("Salary");
            RuleFor(c => c.Salary).LessThanOrEqualTo(9999999).WithName("Salary");
            RuleFor(c => c.ResignedReason).MaximumLength(1024).WithName("Resigned Reason");
        }
    }
}
