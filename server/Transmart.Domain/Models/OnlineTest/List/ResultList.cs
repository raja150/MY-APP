using System;

namespace TranSmart.Domain.Models.OnlineTest.List
{
	public class ResultList : BaseModel
	{
		public string PaperName { get; set; }
		public DateTime Date { get; set; }
		public Guid PaperId { get; set; }
		public Guid EmployeeId { get; set; }
		public string Employee { get; set; }
		public decimal Percentage { get; set; }
		public bool ShowResult { get; set; }
	}
}
