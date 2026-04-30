using DocumentFormat.OpenXml.Drawing.Charts;
using TranSmart.Core.Attributes;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.API.Models.Import
{
	public class IncentivePayCutResultHeadersModel: IncentivesPayCutModel
	{
		[DataImportAttribute(Name = "Incentives", Order = 1)]
		public int Incentives { get; set; }

		[DataImportAttribute(Name = "Pay cut", Order = 2)]
		public int PayCut { get; set; }
	}
}
