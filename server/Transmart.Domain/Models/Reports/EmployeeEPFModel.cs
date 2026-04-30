using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
    public class EmployeeEpfModel : BaseModel
    {
        public string UANNo { get; set; }
        public string MemberName { get; set; }
        public decimal GrossWages { get; set; }
        public decimal NCPDays { get; set; }
        public int? EPFCont { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
		public string EmployeePFNo { get; set; }
		public string EPS { get; set; }
		public string EmployeePF { get; set; }
		public int? EmployeeContrib { get; set; }
	}
}
