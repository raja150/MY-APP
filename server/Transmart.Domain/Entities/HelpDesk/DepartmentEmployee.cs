using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.HelpDesk
{
	[Table("HD_DepartmentEmployee")]
	public class DepartmentEmployee:BaseEntity
	{
		public Guid EmployeeId { get; set; }
		public Guid DeskDepartmentId { get; set; }
		public Guid GroupId { get; set; }	
	}
}
