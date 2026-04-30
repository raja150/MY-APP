using System;
using System.Collections.Generic;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class TestModel : BaseModel
	{
		public string Text { get; set; }
		public byte Type { get; set; }
		public string Answer { get; set; }
		public int TimeSpent { get; set; }
		public ICollection<ChoiceModel> Choices { get; set; }
		public string TestName { get; set; }
		public int Duration { get; set; }
		public DateTime TestDate { get; set; }
	}
}
