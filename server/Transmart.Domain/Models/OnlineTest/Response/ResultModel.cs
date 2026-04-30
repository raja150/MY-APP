using System;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class ResultPaperModel : BaseModel
	{
		public string PaperName { get; set; }
	}

	public class ResultEmpModel : BaseModel
	{
		public string EmployeeName { get; set; }
	}
}
