using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using System;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
	public class AttendanceImportModel
	{
		[DataImport(Name = "Employee Code", Order = 1)]
		public string EmployeeCode { get; set; }

		[DataImport(Name = "Present", Order = 4)]
		public decimal Present { get; set; }

		[DataImport(Name = "LOP", Order = 5)]
		public decimal LOP { get; set; }

		[DataImport(Name = "Unauthorized", Order = 6)]
		public decimal Unauthorized { get; set; }

		[DataImport(Name = "Off Days", Order = 7)]
		public decimal OffDays { get; set; }

		[DataImport(Name = "Error", Order = 8, Required = false, ForError = true)]
		public string Error { get; set; }
	}
}
