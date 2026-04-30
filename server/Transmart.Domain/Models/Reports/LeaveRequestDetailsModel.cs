using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
	public class LeaveRequestDetailsModel : BaseModel
	{
		public DateTime LeaveDate { get; set; }
		public string LeaveName { get; set; }
		public decimal LeaveCount { get;set; }
	}
}
