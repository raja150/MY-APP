
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.SelfService;

namespace TranSmart.Domain.Models.Leave.Model
{
	public partial class UnAuthorizedLeavesModel : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; }
		public Guid RefId { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		public DateTime Date { get; set; }
		public int LeaveStatus { get; set; }
		public string EmployeeStatus { get; set; }
	}
}
