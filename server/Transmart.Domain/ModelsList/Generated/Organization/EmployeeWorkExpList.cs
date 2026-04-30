using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeWorkExpList : BaseModel
    {
        public string Organization { get; set; }
        public string Designation { get; set; }
    }
}
