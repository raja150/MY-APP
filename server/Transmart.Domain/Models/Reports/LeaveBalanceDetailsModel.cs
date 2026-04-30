using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
	public class LeaveBalanceDetailsModel : BaseModel
	{
		public string Name { get; set; }
		public decimal Leaves { get; set; }
		public DateTime? EffectiveFrom { get; set; }
		public DateTime? EffectiveTo { get; set; }
	}
}
