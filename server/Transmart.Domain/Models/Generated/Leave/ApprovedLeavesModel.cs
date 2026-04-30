using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ApprovedLeavesModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid LeaveTypeId { get; set; }
        public bool Allow { get; set; }
        public int? Limit { get; set; }
        [Required]
        public int NoOfLeaves { get; set; }
        public string Reason { get; set; }
    }
    public class ApprovedLeavesModelValidator : AbstractValidator<ApprovedLeavesModel>
    {
        public ApprovedLeavesModelValidator()
        {
            RuleFor(c => c.NoOfLeaves).GreaterThanOrEqualTo(0).WithName("No Of Leaves");
            RuleFor(c => c.NoOfLeaves).LessThanOrEqualTo(999).WithName("No Of Leaves");
            RuleFor(c => c.Reason).MaximumLength(1024).WithName("Reason");
        }
    }
}
