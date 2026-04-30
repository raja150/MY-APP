using System;

namespace TranSmart.Domain.Models.OnlineTest.Response
{
	public class TestDesignationModel : BaseModel
	{
		public Guid PaperId { get; set; }
		public Guid DesignationId { get; set; }

	}
}
