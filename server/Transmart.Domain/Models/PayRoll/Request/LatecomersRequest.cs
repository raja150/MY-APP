using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Request
{
	public class LatecomersRequest : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public Guid? PayMonthId { get; set; }
		public decimal NumberOfDays { get; set; }
	}
}
