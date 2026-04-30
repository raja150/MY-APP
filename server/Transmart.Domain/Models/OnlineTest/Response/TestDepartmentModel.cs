using System;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class TestDepartmentModel : BaseModel
	{
		public Guid PaperId { get; set; }
		public Guid DepartemntId { get; set; }
	}
}
