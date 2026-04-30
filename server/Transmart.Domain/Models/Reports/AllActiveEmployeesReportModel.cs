using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports.Generated
{
	public class AllActiveEmployeesReportModel
	{
		public string EmpCode { get; set; }
		public string Name { get; set; }
		public string PanNumber { get; set; }
		public string MobileNumber { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
		public string PersonalEmail { get; set; }
		public string WorkEmail { get; set; }
		public string Team { get; set; }
		public string WorkLocation { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int Gender { get; set; }
		public string FatherName { get; set; }
		public string AadhaarNumber { get; set; }
		public string ReportingTo { get; set; }
		public string WorkType { get; set; }
		public string EmpCategory { get; set; }
		public string ProfileStatus { get; set; }
		public int? MaritalStatus { get; set; }
		public int? Status { get; set; }
		public string BloodGroup { get; set; }
		public string GenderTxt
		{
			get
			{
				if (Gender == 1) { return "Male"; }
				else
				{ return Gender == 2 ? "Female" : "Other"; }
			}
		}

		public string MaritalStatusTxt
		{
			get
			{
				if (MaritalStatus == 1)
				{
					return "Married";
				}
				else if (MaritalStatus == 2)
				{
					return "Single";
				}
				else
				{
					return MaritalStatus == 3 ? "Seperated" : "";
				}
			}
		}
		public string EmpStatus
		{
			get
			{
				if (Status == 0)
				{
					return "In-Active";
				}
				else if (Status == 1)
				{
					return "Active"; 
				}
				else
				{
					return Status == 3 ? "Resigned" : "";
				}
			}
		}
		public string ProfileStatusTxt
		{
			get
			{
				if (ProfileStatus == "0")
				{
					return "Complete";
				}
				else
				{
					return ProfileStatus == "1" ? "In-Complete" : "";
				}
			}
		}
	}
}
