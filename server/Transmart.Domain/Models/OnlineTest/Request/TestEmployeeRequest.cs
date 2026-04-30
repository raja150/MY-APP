using System;

namespace TranSmart.Domain.Models.OnlineTest.Request
{
	public class TestEmployeeRequest : BaseModel
	{
		public Guid PaperId { get; set; }
		public Guid EmployeeId { get; set; }
	}
}
