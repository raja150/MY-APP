using System;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_TestDepartment")]
	public class TestDepartment : DataGroupEntity
	{
		public Guid PaperId { get; set; }
		public Paper Paper { get; set; }
		public Guid DepartmentId { get; set; }
		public Department Department { get; set; }
		public bool IsDelete { get; set; }
	}
}
