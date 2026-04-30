using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
	public class LatecomersModel
	{
		[DataImport(Name = "Employee Code", Order = 1)]
		public string EmployeeCode { get; set; }

		[DataImport(Name = "Number of Days", Order = 2)]
		public decimal NumberOfdays { get; set; }

		[DataImport(Name = "Error", Order = 3, Required = false, ForError = true)]
		public string Error { get; set; }
	}
}
