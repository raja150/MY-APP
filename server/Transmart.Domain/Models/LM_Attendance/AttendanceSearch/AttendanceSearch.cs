using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.LM_Attendance
{
    public class AttendanceSearch : BaseSearch
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Guid? EmpId { get; set; }
        public Guid? Department { get; set; }
        public Guid? Designation { get; set; }
    }
}
