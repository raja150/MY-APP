using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeeDeviceList : BaseModel
    {
        public int EmployeeId { get; set; }
        public string MobileNumber { get; set; }
        public int ComputerType { get; set; }
        public string HostName { get; set; }
        public bool IsActZeroInstalled { get; set; }
        public bool IsKInstalled { get; set; }
        public bool IsUninstalled { get; set; }
        public DateTime? UninstalledOn { get; set; }
    }
}
