using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Helpdesk
{
    public partial class DeskGroupModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CanEditTicket { get; set; }
        [Required]
        public int CanPostReply { get; set; }
        [Required]
        public int CanCloseTicket { get; set; }
        [Required]
        public int CanAssignTicket { get; set; }
        [Required]
        public int CanTransferTicket { get; set; }
        [Required]
        public int CanDeleteTicket { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class DeskGroupModelValidator : AbstractValidator<DeskGroupModel>
    {
        public DeskGroupModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MinimumLength(2).WithName("Name");
            RuleFor(c => c.Name).MaximumLength(16).WithName("Name");
        }
    }
}
