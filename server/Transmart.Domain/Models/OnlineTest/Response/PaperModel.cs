using System;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class PaperModel : BaseModel
	{
		public string Name { get; set; }
		public Guid OrganiserId { get; set; }
		public Int16 Duration { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public bool IsJumbled { get; set; }
		public bool MoveToLive { get; set; }
		public bool Status { get; set; }
		public bool ShowResult { get; set; }
	}
}
