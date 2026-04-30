using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeContactModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string PersonName { get; set; }
        [Required]
        public int HumanRelation { get; set; }
        [Required]
        public string ContactNo { get; set; }
    }
    public class EmployeeContactModelValidator : AbstractValidator<EmployeeContactModel>
    {
        public EmployeeContactModelValidator()
        {
            RuleFor(m => m.PersonName).NotEmpty().WithName("Person Name");
            RuleFor(c => c.PersonName).MaximumLength(1024).WithName("Person Name");
            RuleFor(m => m.ContactNo).NotEmpty().WithName("Contact Number");
            RuleFor(c => c.ContactNo).MaximumLength(1024).WithName("Contact Number");
        }
    }
}
