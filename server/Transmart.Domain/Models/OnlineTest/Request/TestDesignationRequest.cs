using System;

namespace TranSmart.Domain.Models.OnlineTest.Request
{
	public class TestDesignationRequest : BaseModel
	{
		public Guid PaperId { get; set; }
		public Guid DesignationId { get; set; }
	}
}
