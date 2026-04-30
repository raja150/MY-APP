using System;
using System.Collections.Generic;

namespace TranSmart.Domain.Models.OnlineTest.Request
{
	public class VerifyAnswerRequest : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public bool IsCorrect { get; set; }
	}
	public class CorrectionRequest : BaseModel
	{
		public List<VerifyAnswerRequest> ManualAns { get; set; }
	}
}
