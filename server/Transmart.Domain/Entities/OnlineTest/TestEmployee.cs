using System;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_TestEmployee")]
	public class TestEmployee : DataGroupEntity
	{
		public Guid PaperId { get; set; }
		public Paper Paper { get; set; }
		public Guid EmployeeId { get; set; }
		public Employee Employee { get; set; }
		public bool IsDelete { get; set; }

	}
}
