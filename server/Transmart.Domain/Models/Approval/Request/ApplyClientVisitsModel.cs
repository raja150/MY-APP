using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class ApplyClientVisitsModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? ApprovedById { get; set; }
        [Required]
        public string PlaceOfVisit { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public string PurposeOfVisit { get; set; }

        public string AdminReason { get; set; }
		public int Status { get; set; }
	}
    public class ApplyClientVisitsModelValidator : AbstractValidator<ApplyClientVisitsModel>
    {
        public ApplyClientVisitsModelValidator()
        {
            RuleFor(m => m.PlaceOfVisit).NotEmpty().WithName("Place of Visit");
            RuleFor(c => c.PlaceOfVisit).MaximumLength(1024).WithName("Place of Visit");
            RuleFor(m => m.PurposeOfVisit).NotEmpty().WithName("Purpose of Visit");
            RuleFor(c => c.PurposeOfVisit).MaximumLength(1024).WithName("Purpose of Visit");
            RuleFor(c => c.AdminReason).MaximumLength(1024).WithName("Textarea");
        }
    }
}


