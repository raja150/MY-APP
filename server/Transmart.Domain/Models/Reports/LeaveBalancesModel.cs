using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports.LMS
{
    public partial class LeaveBalancesModel:BaseModel
    {
        public string No { get; set; }
        public string Name { get; set; }
        public decimal Leaves { get; set; }
        public string Designation { get; set; }
        public string LeaveType { get; set; }
        public decimal Balance { get; set; }
        public Guid ReportingToId { get; set; }
    }
}
