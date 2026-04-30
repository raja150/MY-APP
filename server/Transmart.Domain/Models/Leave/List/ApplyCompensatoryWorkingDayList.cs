using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Leave
{
    public partial class ApplyCompensatoryWorkingDayList : BaseModel
    {
        public string EmployeeName { get; set; }
        public string EmployeeNo { get; set; }
        public string Designation { get; set; }
        public Guid? EmployeeId { get; set; }
        public string AdminReason { get; set; }
        public string  Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReasonForApply { get; set; }
        public string Department { get; set; }
        public string ApprovedBy { get; set; }
        public Guid ApprovedById { get; set; }
    }
}
