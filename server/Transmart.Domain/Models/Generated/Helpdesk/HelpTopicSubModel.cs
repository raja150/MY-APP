using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class HelpTopicSubModel : BaseModel
    {
        public Guid HelpTopicId { get; set; }
        [Required]
        public string SubTopic { get; set; }
    }
    public class HelpTopicSubModelValidator : AbstractValidator<HelpTopicSubModel>
    {
        public HelpTopicSubModelValidator()
        {
            RuleFor(m => m.SubTopic).NotEmpty().WithName("SubTopics");
            RuleFor(c => c.SubTopic).MaximumLength(1024).WithName("SubTopics");
        }
    }
}
