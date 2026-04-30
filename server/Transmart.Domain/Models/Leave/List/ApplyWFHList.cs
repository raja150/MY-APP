using System;

namespace TranSmart.Domain.Models.Leave
{
   public partial class ApplyWfhList:BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNo { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string AdminReason { get; set; }
        public DateTime FromCC { get; set; }   
        public string ApprovedBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
		public string ReportingTo { get; set; }
	}
}
