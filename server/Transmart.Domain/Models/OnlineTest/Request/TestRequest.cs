using System;
using System.Collections.Generic;

namespace TranSmart.Domain.Models.OnlineTest.Request
{
	public class TestRequest : BaseModel
	{
		//public Guid TestEmpId { get; set; }
		public Guid PaperId { get; set; }
		public bool IsFinish { get; set; }
		public List<TestAnswerRequest> Answer { get; set; }
	}
	public class TestAnswerRequest : BaseModel
	{
		public string Answer { get; set; }
		public int TimeSpent { get; set; }
	}
}
