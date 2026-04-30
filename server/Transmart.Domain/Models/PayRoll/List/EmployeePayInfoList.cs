using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.List
{
	public partial class EmployeePayInfoList : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string Employee { get; set; }
		public string No { get; set; }
		public int PayMode { get; set; }
	}
}
