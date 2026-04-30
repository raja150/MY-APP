using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Entities.SelfService.Reports

{
	public class EmployeeProfileQry : BaseEntity
	{

		public string No { get; set; }
		public string Name { get; set; }
		public string MobileNumber { get; set; }
		public int Gender { get; set; }
		public string Department { get; set; }
		public DateTime DateOfBirth { get; set; }
		public DateTime DateOfJoining { get; set; }
		public string Designation { get; set; }
		public string WorkType { get; set; }
		public string ReportingTo { get; set; }
		public string PersonalEmail { get; set; }
		public string AadhaarNumber { get; set; }
		public string PanNumber { get; set; }
		public int? MaritalStatus { get; set; }
	}
}
