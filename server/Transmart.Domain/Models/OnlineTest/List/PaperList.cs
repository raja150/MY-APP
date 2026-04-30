using System;

namespace TranSmart.Domain.Models.OnlineTest.List
{
	public class PaperList : BaseModel
	{
		public string Name { get; set; }
		public int Duration { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public bool Status { get; set; }
		public bool MoveToLive { get; set; }

	}
}
