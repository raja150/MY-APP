using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
    public class EmpProfileModel : BaseModel
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public int? MaritalStatus { get; set; }
        public string MobileNumber { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public Guid? WorkLocationId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? DesignationId { get; set; }
        public string PassportNumber { get; set; }
        public string PanNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string WorkLocation { get; set; }
        public string EmployeeTeam { get; set; }
    }
}
