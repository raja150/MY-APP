using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ExemptionsModel : BaseModel
    {
        public Guid HolidaysId { get; set; }
        public string Employees { get; set; }
        public string Department { get; set; }
        public string Designations { get; set; }
        public string Teams { get; set; }
        public string Location { get; set; }
    }
    public class ExemptionsModelValidator : AbstractValidator<ExemptionsModel>
    {
        public ExemptionsModelValidator()
        {
            RuleFor(c => c.Employees).MaximumLength(1024).WithName("Employees");
            RuleFor(c => c.Department).MaximumLength(1024).WithName("Department");
            RuleFor(c => c.Designations).MaximumLength(1024).WithName("Designations");
            RuleFor(c => c.Teams).MaximumLength(1024).WithName("Teams");
            RuleFor(c => c.Location).MaximumLength(1024).WithName("Location");
        }
    }
}
