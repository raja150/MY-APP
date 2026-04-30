using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeResignationModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public int ResignationType { get; set; }
        [Required]
        public DateTime ResignationOn { get; set; }
        [Required]
        public DateTime ApprovedOn { get; set; }
        [Required]
        public string LeavingReason { get; set; }
        [Required]
        public string AllowRehiring { get; set; }
        public string Description { get; set; }
    }
    public class EmployeeResignationModelValidator : AbstractValidator<EmployeeResignationModel>
    {
        public EmployeeResignationModelValidator()
        {
            RuleFor(m => m.LeavingReason).NotEmpty().WithName("Leaving Reason");
            RuleFor(c => c.LeavingReason).MaximumLength(1024).WithName("Leaving Reason");
            RuleFor(m => m.AllowRehiring).NotEmpty().WithName("Allow for Rehiring");
            RuleFor(c => c.AllowRehiring).MaximumLength(1024).WithName("Allow for Rehiring");
            RuleFor(c => c.Description).MaximumLength(1024).WithName("Description");
        }
    }
}
