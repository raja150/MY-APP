using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.Organization
{
	[Table("Org_EmployeeDevice")]
	public class EmployeeDevice : DataGroupEntity
	{
		public Guid EmployeeId { get; set; }
		public Organization.Employee Employee { get; set; }
		public string MobileNumber { get; set; }
		public int ComputerType { get; set; }
		public string HostName { get; set; }
		public bool IsActZeroInstalled { get; set; }
		public bool IsK7Installed { get; set; }
		public bool IsUninstalled { get; set; }
		public Guid? InstalledById { get; set; }
		public Employee InstalledBy { get; set; }
		public Guid? UninstalledById { get; set; }
		public Employee UninstalledBy { get; set; }
		public DateTime? InstalledOn { get; set; }
		public DateTime? UninstalledOn { get; set; }
	}
}
