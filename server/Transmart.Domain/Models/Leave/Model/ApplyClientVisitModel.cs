using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Model
{
    public class ApplyClientVisitModel :  BaseModel
    {
        public Guid EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public string EmployeeNo { get; set; }
		public string Designation { get; set; }
        public Guid? ApprovedById { get; set; }
        public string PlaceOfVisit { get; set; }
        public string EmailID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PurposeOfVisit { get; set; }
        public int Status { get; set; }
        public string AdminReason { get; set; }
		public string Reason { get; set; }
		public string RejectReason { get; set; }
	}
}
