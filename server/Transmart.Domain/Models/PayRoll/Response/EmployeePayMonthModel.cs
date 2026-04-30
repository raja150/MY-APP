using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{

    public class EmployeePayMonthModel : BaseModel
    {
		public Guid PayMonthId { get; set; }
        public string Name { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }

	}
}
