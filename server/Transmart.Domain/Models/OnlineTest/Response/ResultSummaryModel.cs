using System;
using System.Collections.Generic;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class ResultSummaryModel : BaseModel
	{
		public string Name { get; set; }
		public int TotalQuestion { get; set; }
		public int Answered { get; set; }
		public int Correct { get; set; }
		public int Wrong { get; set; }
		public DateTime TestDate { get; set; }
		public decimal Percentage { get; set; }
		public int TotalTime { get; set; }
		public int TimeSpent { get; set; }
		public int UnAnswered { get; set; }
		public Guid PaperId { get; set; }
		public Guid EmpId { get; set; }
		public string EmpName { get; set; }
		public IEnumerable<QuestionAnswerModel> QueAnswerModel { get; set; }
	}
}
