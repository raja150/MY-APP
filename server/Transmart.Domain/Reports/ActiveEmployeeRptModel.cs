using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Reports
{
	public class ActiveEmployeeRptModel
	{
		public Guid? DepartmentId { get; set; }
		public Guid? DesignationId { get; set; }
		public Guid? TeamId { get; set; }
		public Guid? EmployeeId { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public Guid? WorkTypeId { get; set; }
		public Guid? EmpCategoryId { get; set; }
	}
}
