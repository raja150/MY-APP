using System;

namespace TranSmart.Domain.Models.OnlineTest.List
{
	public class CorrectionList : BaseModel
	{
		public string Test { get; set; }
		public int TotalAttendees { get; set; }
		public DateTime TestOn { get; set; }
		public Guid EmployeeId { get; set; }
	}
}
