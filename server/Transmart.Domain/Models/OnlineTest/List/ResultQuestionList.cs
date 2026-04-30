using System;

namespace TranSmart.Domain.Models.OnlineTest.List
{
	public class ResultQuestionList : BaseModel
	{
		public string PaperName { get; set; }
		public DateTime TestDate { get; set; }
		public int TimeSpent { get; set; }

	}
}
