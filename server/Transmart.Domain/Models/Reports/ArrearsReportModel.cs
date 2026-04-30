using System;

namespace TranSmart.Domain.Models.Reports
{
	public partial class ArrearsReportModel : BaseModel
	{
		public string EmpCode { get; set; }
		public string Name { get; set; }
		public string Designation { get; set; }
		public DateTime DOJ { get; set; }
		public int Month { get; set; }
		public decimal TaxAmount { get; set; }
	}
}
