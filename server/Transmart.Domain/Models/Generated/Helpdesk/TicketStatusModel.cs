using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class TicketStatusModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int OrderNo { get; set; }
        public bool IsClosed { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class TicketStatusModelValidator : AbstractValidator<TicketStatusModel>
    {
        public TicketStatusModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(16).WithName("Name");
            RuleFor(c => c.OrderNo).GreaterThanOrEqualTo(0).WithName("Display Order");
            RuleFor(c => c.OrderNo).LessThanOrEqualTo(99).WithName("Display Order");
        }
    }
}
