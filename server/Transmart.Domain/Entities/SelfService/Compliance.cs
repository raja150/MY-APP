using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.SelfService
{
	[Table("SS_ComplianceDoc")]
	public class Compliance : DataGroupEntity
	{
		public Guid EmployeeId { get; set; }
		public Organization.Employee Employee { get; set; }
		public string FileName { get; set; }		
	}
}
