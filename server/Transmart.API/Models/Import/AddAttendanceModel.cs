using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
	public class AddAttendanceModel
	{
		[DataImport(Name = "Employee Code", Order = 1, Required = true)]
		public string EmpCode { get; set; }

		[DataImport(Name = "Attendance Date", Order = 2, Required = true)]
		public DateTime AttendanceDate { get; set; }
		[DataImport(Name = "Error", Order = 10, Required = false, ForError = true)]
		public string Error { get; set; }
	}
}
