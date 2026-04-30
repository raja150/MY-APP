using System;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Models.OnlineTest.Request
{
	public class TestDepartmentRequest : BaseModel
	{
		public Guid PaperId { get; set; }
		public Guid DepartmentId { get; set; }
	}
	
}
