using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Leave
{
    public partial class LeaveTypeScheduleModel : BaseModel
    {
        public Guid LeaveTypeId { get; set; }
        [Required]
        public int AccType { get; set; }
        [Required]
        public int AccOnDay { get; set; }
        public int? AccOnYearly { get; set; }
        public int? AccOnHalfYearly { get; set; }
        public int? AccOnQuarterly { get; set; }
        [Required]
        public int NoOfDays { get; set; }
        [Required]
        public int ResetType { get; set; }
        [Required]
        public int ResOnDay { get; set; }
        public int? ResOnYearly { get; set; }
        public int? ResOnHalfYearly { get; set; }
        public int? ResOnQuarterly { get; set; }
        [Required]
        public int ResetNoOfDays { get; set; }
        [Required]
        public int FwdType { get; set; }
        public int? FwdPercentage { get; set; }
        public int? FwdDays { get; set; }
        [Required]
        public int FwdLimit { get; set; }
        public int? FwdOverallLimit { get; set; }
        public int? OpeningBalance { get; set; }
        public int? MaxBalance { get; set; }
    }
    public class LeaveTypeScheduleModelValidator : AbstractValidator<LeaveTypeScheduleModel>
    {
        public LeaveTypeScheduleModelValidator()
        {
            RuleFor(c => c.NoOfDays).GreaterThanOrEqualTo(0).WithName("No. of Days");
            RuleFor(c => c.NoOfDays).LessThanOrEqualTo(99).WithName("No. of Days");
            RuleFor(c => c.ResetNoOfDays).GreaterThanOrEqualTo(0).WithName("No. of Days");
            RuleFor(c => c.ResetNoOfDays).LessThanOrEqualTo(99).WithName("No. of Days");
            RuleFor(c => c.FwdPercentage).GreaterThanOrEqualTo(0).WithName("Percent");
            RuleFor(c => c.FwdPercentage).LessThanOrEqualTo(999).WithName("Percent");
            RuleFor(c => c.FwdDays).GreaterThanOrEqualTo(0).WithName("Days");
            RuleFor(c => c.FwdDays).LessThanOrEqualTo(999).WithName("Days");
            RuleFor(c => c.FwdLimit).GreaterThanOrEqualTo(0).WithName("Limit");
            RuleFor(c => c.FwdLimit).LessThanOrEqualTo(999).WithName("Limit");
            RuleFor(c => c.FwdOverallLimit).GreaterThanOrEqualTo(0).WithName("Overall Limit");
            RuleFor(c => c.FwdOverallLimit).LessThanOrEqualTo(999).WithName("Overall Limit");
            RuleFor(c => c.OpeningBalance).GreaterThanOrEqualTo(0).WithName("Opening Balance");
            RuleFor(c => c.OpeningBalance).LessThanOrEqualTo(999).WithName("Opening Balance");
            RuleFor(c => c.MaxBalance).GreaterThanOrEqualTo(0).WithName("Maximum Balance");
            RuleFor(c => c.MaxBalance).LessThanOrEqualTo(999).WithName("Maximum Balance");
        }
    }
}
