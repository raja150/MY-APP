using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.List
{
   public class SalaryList:BaseModel
    {
		public Guid EmployeeId { get; set; }
		public string EmpName { get; set; }
        public string EmpNo { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int Salary { get; set; }
        public string MobileNo { get; set; }   
        public DateTime? ModifiedAt { get; set; }


    }
}
