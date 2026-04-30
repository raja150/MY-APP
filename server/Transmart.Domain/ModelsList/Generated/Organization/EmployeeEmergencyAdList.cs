using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeEmergencyAdList : BaseModel
    {
        public string State { get; set; }
        public string Country { get; set; }
    }
}
