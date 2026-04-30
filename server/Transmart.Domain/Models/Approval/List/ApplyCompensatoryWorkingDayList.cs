using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class ApplyCompensatoryWorkingDayList : BaseModel
    {
        public Guid? ShiftId { get; set; }
        public string EmployeeName { get; set; }
        public Guid? EmployeeId { get; set; }
        public string AdminReason { get; set; }
        public int Status { get; set; }
        public DateTime To { get; set; }
        public string EmailID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime From { get; set; }
        public string ReasonForApply { get; set; }
    }
}
