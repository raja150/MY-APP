using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class EmpStatutoryList : BaseModel
    {
        public string Emp { get; set; }
        public string EmployeesProvid { get; set; }
        public string UAN { get; set; }
        public int? EmployeeContrib { get; set; }
        public bool EPS { get; set; }
        public string ESINo { get; set; }
    }
}
