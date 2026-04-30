using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
    public partial class AllocationList
    {
        public string EmployeeName { get; set; }
        public string No { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
