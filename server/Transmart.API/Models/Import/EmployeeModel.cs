using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class EmployeeModel
    {
        [DataImport(Name = "Employee Code", Order = 1, Required = true)]
        public string EmpCode { get; set; }

		[DataImport(Name = "Employee Name", Order = 2)]
        public string Name { get; set; }

		[DataImport(Name = "Mobile Number", Order = 3)]
        public string MobileNumber { get; set; }

        [DataImport(Name = "Gender", Order = 4)]
        public string Gender { get; set; }

        [DataImport(Name = "Date Of Birth", Order = 5)]
        public DateTime? DateOfBirth { get; set; }

		[DataImport(Name = "Date Of Joining", Order = 6)]
        public DateTime? DateOfJoining { get; set; }
		[DataImport(Name = "Department", Order = 7)]
        public string Department { get; set; }

        [DataImport(Name = "Designation", Order = 8)]
        public string Designation { get; set; }
		[DataImport(Name = "Team", Order = 9)]
		public string Team { get; set; }
		[DataImport(Name = "Work Type", Order = 10)]
		public string WorkType { get; set; }
		[DataImport(Name = "Work Location", Order = 11)]
        public string WorkLocation { get; set; }

        [DataImport(Name = "Aadhaar Number", Order = 12)]
        public string AadhaarNumber { get; set; }

        [DataImport(Name = "Work From Home", Order = 13)]
        public string WorkFromHome { get; set; }

		[DataImport(Name = "First Name", Order = 14)]
		public string FirstName { get; set; }
		[DataImport(Name = "Middle Name", Order = 15, Required = false)]
		public string MiddleName { get; set; }
		[DataImport(Name = "Last Name", Order = 16, Required = false)]
		public string LastName { get; set; }

		[DataImport(Name = "DOB As Per Certificate", Order = 17)]
		public DateTime? DOBC { get; set; }

		[DataImport(Name = "Marital Status", Order = 18)]
		public string MaritalStatus { get; set; }
		[DataImport(Name = "Line Of Business", Order = 19, Required = false)]
		public string LOB { get; set; }
		[DataImport(Name = "Functional Area", Order = 20, Required = false)]
		public string FunctionalArea { get; set; }

		[DataImport(Name = "Status", Order = 21)]
		public string Status { get; set; }

		[DataImport(Name = "Error", Order = 22, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
