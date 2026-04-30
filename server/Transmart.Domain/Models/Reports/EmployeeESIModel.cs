using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
    public class EmployeeEsiModel : BaseModel
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
        public string ESINo { get; set; }
        public decimal GrossWage { get; set; }
        public decimal LOPDays { get; set; }
        public decimal ESIDeduction { get; set; }
        public string Branch { get; set; }
		public string UANNo { get; set; }
		public string EPS { get; set; }
		public string EmployeeESI { get; set; }


	}
}
