using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class LocationModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public Guid StateId { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? WeekOffSetupId { get; set; }
        public Guid? WorkHoursSettingId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class LocationModelValidator : AbstractValidator<LocationModel>
    {
        public LocationModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
        }
    }
}
