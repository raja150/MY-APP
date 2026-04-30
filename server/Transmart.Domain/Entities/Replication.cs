using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities
{
	[Table("_Replication")]
	public class Replication : DataGroupEntity
	{
		/// <summary>
		/// Added / Updated 1 and 2 
		/// </summary>
		public byte Type { get; set; }
		public Guid? DepartmentId { get; set; }

		/// <summary>
		/// Designation
		/// Employee
		/// Salary
		/// </summary>
		public byte Category { get; set; }
		/// <summary>
		/// Pk of Category
		/// </summary>
		public Guid RefId { get; set; }
		public bool Status { get; set; }
	}
}
