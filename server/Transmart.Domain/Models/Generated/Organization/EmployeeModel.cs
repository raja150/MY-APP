using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeModel : BaseModel
    {
        [Required]
        public string No { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime DateOfJoining { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime? LastWorkingDate { get; set; }
        public Guid DesignationId { get; set; }
        public Guid WorkLocationId { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid TeamId { get; set; }
        public Guid? ReportingToId { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string BloodGroup { get; set; }
        [Required]
        public string AadhaarNumber { get; set; }
        public string PanNumber { get; set; }
        public string PassportNumber { get; set; }
        public int? MaritalStatus { get; set; }
        public DateTime? MarriageDay { get; set; }
        public string FatherName { get; set; }
        [Required]
        public int Status { get; set; }
        public string ProfileStatus { get; set; }
        public Guid? EmpCategoryId { get; set; }
        public bool AllowWebPunch { get; set; }
        public Guid? ProbationPeriodId { get; set; }
        [Required]
        public int WorkFromHome { get; set; }
        [Required]
        public DateTime DOBC { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Guid? LOBId { get; set; }
        public Guid? FunctionalAreaId { get; set; }
    }
    public class EmployeeModelValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeModelValidator()
        {
            RuleFor(m => m.No).NotEmpty().WithName("No");
            RuleFor(c => c.No).MaximumLength(1024).WithName("No");
            RuleFor(m => m.Name).NotEmpty().WithName("Name");
            RuleFor(c => c.Name).MaximumLength(1024).WithName("Name");
            RuleFor(m => m.MobileNumber).NotEmpty().WithName("Mobile Number");
            RuleFor(c => c.MobileNumber).MaximumLength(1024).WithName("Mobile Number");
            RuleFor(c => c.WorkEmail).MaximumLength(1024).WithName("Work Email");
            RuleFor(c => c.PersonalEmail).MaximumLength(1024).WithName("Personal Email");
            RuleFor(c => c.BloodGroup).MaximumLength(1024).WithName("Blood Group");
            RuleFor(m => m.AadhaarNumber).NotEmpty().WithName("Aadhaar Number");
            RuleFor(m => m.AadhaarNumber).Matches(Resource.RegEx_Aadhar).WithName("Aadhaar Number");
            RuleFor(c => c.AadhaarNumber).MaximumLength(1024).WithName("Aadhaar Number");
            RuleFor(m => m.PanNumber).Matches(Resource.RegEx_PAN).When(x => x.PanNumber.Length > 0).WithName("PAN");
            RuleFor(c => c.PanNumber).MaximumLength(1024).WithName("PAN");
            RuleFor(c => c.PassportNumber).MaximumLength(1024).WithName("Passport Number");
            RuleFor(c => c.FatherName).MaximumLength(1024).WithName("Father Name");
            RuleFor(c => c.ProfileStatus).MaximumLength(1024).WithName("Profile Status");
            RuleFor(m => m.FirstName).NotEmpty().WithName("First Name");
            RuleFor(c => c.FirstName).MaximumLength(1024).WithName("First Name");
            RuleFor(c => c.MiddleName).MaximumLength(1024).WithName("Middle Name");
            RuleFor(c => c.LastName).MaximumLength(1024).WithName("Last Name/Surname");
        }
    }
}
