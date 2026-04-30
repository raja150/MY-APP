using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.LM_Attendance
{
    public class BiometricAttLogsRequest
    {
        public string EmpCode { get; set; }
        public DateTime MovementTime { get; set; }
        public int MovementType { get; set; }
    }
}
