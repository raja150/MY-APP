using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
	public class PaymentInfoModel
	{

		[DataImport(Name = "Employee Code", Order = 1, Required = true)]
		public string EmpCode { get; set; }

		[DataImport(Name = "Pay Mode", Order = 2, Required = true)]
		public string PayMode { get; set; }

		[DataImport(Name = "Employer Bank", Order = 3, Required = true)]
		public string Bank { get; set; }

		[DataImport(Name = "Bank Name", Order = 4)]
		public string BankName { get; set; }

		[DataImport(Name = "IFSC Code", Order = 5)]
		public string IFSCCode { get; set; }

		[DataImport(Name = "Account No", Order = 6)]
		public string AccountNo { get; set; }
		[DataImport(Name = "Error", Order = 7, Required = false, ForError = true)]
		public string Error { get; set; }


	}
}
