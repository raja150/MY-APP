using System;

namespace TranSmart.Domain.Models.Organization
{
	public class EmployeeDeviceModel : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string EmployeeNo { get; set; }
		public string Name { get; set; }
		public string MobileNumber { get; set; }
		public int ComputerType { get; set; }
		public string HostName { get; set; }
		public bool IsActZeroInstalled { get; set; }
		public bool IsK7Installed { get; set; }
		public bool IsUninstalled { get; set; }
		public DateTime? InstalledOn { get; set; }
		public DateTime? UninstalledOn { get; set; }
		public bool addDuplicate { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }

	}
}
