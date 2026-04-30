using System;
using TranSmart.Core.Attributes;

namespace Transmart.Controller.UnitTests.ClosedXml
{
	public class TestModel
	{
		[DataImport(Name = "Code", Order = 1)]
		public string EmployeeCode { get; set; }
		[DataImport(Name = "Age", Order = 2)]
		public int Age { get; set; }
		[DataImport(Name = "Status", Order = 3)]
		public bool Status { get; set; }
		[DataImport(Name = "Dob", Order = 4)]
		public DateTime DOB { get; set; }
		[DataImport(Name = "Salary", Order = 5)]
		public decimal Salary { get; set; }
		[DataImport(Name = "WorkType", Order = 6)]
		public byte WorkType { get; set; }

		[DataImport(Name = "Gender", Order = 7)]
		public short Gender { get; set; }


		[DataImport(Name = "Department", Order = 8)]
		public string? Department { get; set; }
		[DataImport(Name = "Doj", Order = 9)]
		public DateTime? DOJ { get; set; }

		[DataImport(Name = "EmpStatus", Order = 10)]
		public bool? EmpStatus { get; set; }
		[DataImport(Name = "EmpSalary", Order = 11)]
		public decimal? EmpSalary { get; set; }

		[DataImport(Name = "EmpWorkType", Order = 12)]
		public byte? EmpWorkType { get; set; }
		[DataImport(Name = "EmpGender", Order = 13)]
		public short? EmpGender { get; set; }

		[DataImport(Name = "BloodGroup", Order = 14)]
		public int? BloodGroup { get; set; }
	}
}
