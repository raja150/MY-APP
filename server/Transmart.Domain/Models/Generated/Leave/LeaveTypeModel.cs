using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class LeaveTypeModel : BaseModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Duration { get; set; }
        public string Description { get; set; }
        [Required]
        public int PayType { get; set; }
        [Required]
        public bool DefaultPayoff { get; set; }
        [Required]
        public int EffectiveAfter { get; set; }
        [Required]
        public int EffectiveType { get; set; }
        [Required]
        public int EffectiveBy { get; set; }
        [Required]
        public int ProrateByT { get; set; }
        [Required]
        public int RoundOff { get; set; }
        [Required]
        public int RoundOffTo { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public int MaritalStatus { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ExDepartment { get; set; }
        public string ExDesignation { get; set; }
        public string ExLocation { get; set; }
        [Required]
        public int PastDate { get; set; }
        [Required]
        public int FutureDate { get; set; }
        [Required]
        public decimal MinLeaves { get; set; }
        [Required]
        public decimal MaxLeaves { get; set; }
        public decimal? MaxApplications { get; set; }
        public int? specifiedperio { get; set; }
        public bool ProofNeeded { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class LeaveTypeModelValidator : AbstractValidator<LeaveTypeModel>
    {
        public LeaveTypeModelValidator()
        {
            RuleFor(m => m.Code).NotEmpty().WithName("Code");
            RuleFor(c => c.Code).MaximumLength(1024).WithName("Code");
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(c => c.Description).MaximumLength(1024).WithName("Description");
            RuleFor(c => c.EffectiveAfter).GreaterThanOrEqualTo(0).WithName("Effective After");
            RuleFor(c => c.EffectiveAfter).LessThanOrEqualTo(999).WithName("Effective After");
            RuleFor(c => c.Location).MaximumLength(1024).WithName("Location");
            RuleFor(c => c.Department).MaximumLength(1024).WithName("Department");
            RuleFor(c => c.Designation).MaximumLength(1024).WithName("Designation");
            RuleFor(c => c.ExDepartment).MaximumLength(1024).WithName("Department");
            RuleFor(c => c.ExDesignation).MaximumLength(1024).WithName("Designation");
            RuleFor(c => c.ExLocation).MaximumLength(1024).WithName("Location");
            RuleFor(c => c.PastDate).GreaterThanOrEqualTo(0).WithName("Past dates (In days)");
            RuleFor(c => c.PastDate).LessThanOrEqualTo(999).WithName("Past dates (In days)");
            RuleFor(c => c.FutureDate).GreaterThanOrEqualTo(0).WithName("Future dates (In Days)");
            RuleFor(c => c.FutureDate).LessThanOrEqualTo(999).WithName("Future dates (In Days)");
            RuleFor(c => c.MinLeaves).GreaterThanOrEqualTo(0).WithName("Minimum leave that can be availed per application");
            RuleFor(c => c.MinLeaves).LessThanOrEqualTo(99).WithName("Minimum leave that can be availed per application");
            RuleFor(c => c.MaxLeaves).GreaterThanOrEqualTo(0).WithName("Maximum leave that can be availed per application");
            RuleFor(c => c.MaxLeaves).LessThanOrEqualTo(999).WithName("Maximum leave that can be availed per application");
            RuleFor(c => c.MaxApplications).GreaterThanOrEqualTo(0).WithName("Maximum number of applications allowed in specified period");
            RuleFor(c => c.MaxApplications).LessThanOrEqualTo(99).WithName("Maximum number of applications allowed in specified period");
        }
    }
}
