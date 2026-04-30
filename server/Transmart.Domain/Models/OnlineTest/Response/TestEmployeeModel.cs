using System;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class TestEmployeeModel : BaseModel
	{
		public Guid PaperId { get; set; }
		public Guid EmployeeId { get; set; }
	}
}
