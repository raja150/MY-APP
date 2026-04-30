using System;
using System.Collections.Generic;

namespace TranSmart.Domain.Models.OnlineTest
{
	public class CorrectionModel : BaseModel
	{
		public string Question { get; set; }
		public string CorrectAns { get; set; }
		public List<AnswerModel> Answer { get; set; }
	}

	public class AnswerModel : BaseModel
	{
		public string AnswerTxt { get; set; }
		public Guid EmployeeId { get; set; }
	}
}
