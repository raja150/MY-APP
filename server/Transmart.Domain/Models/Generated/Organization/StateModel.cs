using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class StateModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        public string TimeZone { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class StateModelValidator : AbstractValidator<StateModel>
    {
        public StateModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(64).WithName("Name");
            RuleFor(m => m.Country).NotEmpty().WithName("Country");
            RuleFor(c => c.Country).MinimumLength(2).WithName("Country");
            RuleFor(c => c.Country).MaximumLength(64).WithName("Country");
            RuleFor(c => c.TimeZone).MaximumLength(1024).WithName("TimeZone");
        }
    }
}
