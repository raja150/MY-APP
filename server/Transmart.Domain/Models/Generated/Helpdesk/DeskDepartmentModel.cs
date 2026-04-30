using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class DeskDepartmentModel : BaseModel
    {
        [Required]
        public string Department { get; set; }
        public Guid ManagerId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class DeskDepartmentModelValidator : AbstractValidator<DeskDepartmentModel>
    {
        public DeskDepartmentModelValidator()
        {
            RuleFor(m => m.Department).NotEmpty().WithName("Department");
            RuleFor(c => c.Department).MinimumLength(2).WithName("Department");
            RuleFor(c => c.Department).MaximumLength(32).WithName("Department");
        }
    }
}
