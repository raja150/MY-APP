using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class TeamModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? WeekOffSetupId { get; set; }
        public Guid? WorkHoursSettingId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class TeamModelValidator : AbstractValidator<TeamModel>
    {
        public TeamModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(c => c.Description).MaximumLength(1024).WithName("Description");
        }
    }
}
