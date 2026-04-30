using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Approval
{
    public class InfoModel :BaseModel
    {
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; } 
        public byte Status { get; set; }
        public string Reason { get; set; }
        public string RejectReason { get; set; }
    }
}
