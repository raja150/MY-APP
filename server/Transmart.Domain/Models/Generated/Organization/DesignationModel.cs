using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class DesignationModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? WeekOffSetupId { get; set; }
        public Guid? WorkHoursSettingId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class DesignationModelValidator : AbstractValidator<DesignationModel>
    {
        public DesignationModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
