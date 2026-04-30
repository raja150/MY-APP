using System;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_TestDesignation")]
	public class TestDesignation :DataGroupEntity
	{
		public Guid PaperId { get; set; }
		public Paper Paper { get; set; }
		public Guid DesignationId { get; set; }
		public Designation Designation { get; set; }
		public bool IsDelete { get; set; }
	}
}
