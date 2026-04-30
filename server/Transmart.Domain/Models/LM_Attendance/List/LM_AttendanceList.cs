using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.LM_Attendance.List
{
    public class AttendanceList : BaseModel
    {
        public string No { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
