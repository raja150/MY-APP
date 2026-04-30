using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.Response
{
	public class LatecomersModel  : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string EmployeeNo { get; set; }
		public string Name { get; set; }
		public decimal NumberOfDays { get; set; }
	}
}
