using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeEducationList : BaseModel
    {
        public string Qualification { get; set; }
        public string Degree { get; set; }
        public string Medium { get; set; }
        public string Institute { get; set; }
        public decimal Percentage { get; set; }
        public int YearOfPassing { get; set; }
    }
}
