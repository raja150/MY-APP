using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class HolidaysModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public string Description { get; set; }
        public string ReprApplication { get; set; }
        public int? Duration { get; set; }
    }
    public class HolidaysModelValidator : AbstractValidator<HolidaysModel>
    {
        public HolidaysModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(m => m.Description).NotEmpty().WithName("Description");
            RuleFor(c => c.Description).MaximumLength(1024).WithName("Description");
            RuleFor(c => c.ReprApplication).MaximumLength(1024).WithName("Reprocess Application");
        }
    }
}
