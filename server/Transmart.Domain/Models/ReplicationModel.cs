using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models
{
	public class ReplicationModel : BaseModel
	{
		public Guid? DepartmentId { get; set; }
		public byte Type { get; set; }
		public byte Category { get; set; }
		public Guid RefId { get; set; }
		public bool Status { get; set; }
	}
}
