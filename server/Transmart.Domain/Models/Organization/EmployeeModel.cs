using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeAddModel : EmployeeModel
    {
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string WorkLocation { get; set; }
        public string WorkType { get; set; }
        public string EmployeeTeam { get; set; }
        public string ReportingTo { get; set; }

    }
    public class EmployeeManual : AbstractValidator<EmployeeModel>
    {
        public EmployeeManual()
        {
            RuleFor(x => x.DateOfJoining).GreaterThan(x => x.DateOfBirth).WithMessage(Resource.Date_Of_Joining_Should_Be_Greater_Than_Date_Of_Birth);
            RuleFor(x => x.LastWorkingDate).GreaterThan(x => x.DateOfJoining).WithMessage(Resource.Date_Of_Last_Working_Should_Be_Greater_Than_Date_Of_Joining);
            RuleFor(x => x.MarriageDay).GreaterThan(x => x.DateOfBirth).WithMessage(Resource.Marriage_anniversary_should_be_greater_than_date_of_birth);
            RuleFor(x => x.AadhaarNumber).Length(12).WithMessage(Resource.Aadhaar_Must_Be_12_Numbers);
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now).WithMessage(Resource.Date_of_birth_should_be_less_than_current_date);
            RuleFor(x => x.DateOfJoining).LessThanOrEqualTo(DateTime.Now).WithMessage(Resource.Date_of_joining_should_be_less_than_current_date);
			RuleFor(m => m.LastWorkingDate).NotNull().When(x => x.Status == 2).WithMessage(Resource.Last_Working_Date_Is_Required);
		}
    }
    public class EmployeesMailModel : BaseModel
    {
        public string Email { get; set; }
        public string TicketNo { get; set; }
        public ICollection<RecipientsMailsModel> Recipients { get; set; }
    }
    public class RecipientsMailsModel
    {
        public string Mail { get; set; }
    }
}
