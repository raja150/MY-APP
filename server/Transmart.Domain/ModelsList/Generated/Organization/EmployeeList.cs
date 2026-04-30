using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeList : BaseModel
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string WorkLocation { get; set; }
        public string WorkType { get; set; }
        public string Team { get; set; }
        public string WorkEmail { get; set; }
        public int Status { get; set; }
        public string ProfileStatus { get; set; }
        public int WorkFromHome { get; set; }
    }
}
