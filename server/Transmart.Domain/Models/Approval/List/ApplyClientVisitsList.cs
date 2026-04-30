using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
   public partial class ApplyClientVisitsList : BaseModel
    {
        public string EmployeeNo { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string PlaceOfVisit { get; set; }    
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PurposeOfVisit { get; set; }
		public string ApprovedBy { get; set; }

	}
}
