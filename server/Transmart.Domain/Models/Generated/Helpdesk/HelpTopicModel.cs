using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class HelpTopicModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int DueHours { get; set; }
        [Required]
        public int Priority { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid TicketStatusId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class HelpTopicModelValidator : AbstractValidator<HelpTopicModel>
    {
        public HelpTopicModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(16).WithName("Name");
            RuleFor(c => c.DueHours).GreaterThanOrEqualTo(1).WithName("Due Hours");
            RuleFor(c => c.DueHours).LessThanOrEqualTo(999).WithName("Due Hours");
        }
    }
}
