using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.LM_Attendance
{
	public class WebAttendanceSearch : BaseSearch
	{
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public int Status { get; set; }
	}
}
