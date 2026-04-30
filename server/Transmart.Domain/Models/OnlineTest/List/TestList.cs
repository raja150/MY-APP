using System;

namespace TranSmart.Domain.Models.OnlineTest.List
{
	public class TestList : BaseModel
	{
		public string Name { get; set; }
		public int Duration { get; set; }
		public DateTime StartAt { get; set; }
		//public Guid PaperId { get; set; }
		public DateTime EndAt { get; set; }
	}
}
