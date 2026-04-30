using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Organization
{
   public class EmployeeInfoModel:BaseModel
    {
        public Guid DepartmentId { get; set; }//for Unit testing in controller
        public string Name { get; set; }
        public string EmployeeNo { get; set; }
        public string MobileNumber { get; set; }    
        public string Department { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Designation { get; set; }
        public string WorkEmail { get; set; }
        public string GenderTxt
        {
            get
            {
                return Gender switch
                {
                    1 => "M",
                    _ => "F",
                };
            }
        }
    }
}
