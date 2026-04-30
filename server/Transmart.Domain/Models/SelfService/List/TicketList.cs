using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.SelfService.List  
{
    public partial class TicketList : BaseModel
    {
		public string EmployeeNo { get; set; }
		public string Designation { get; set; }
		public string EmpDept { get; set; }		
		public string No { get; set; }
        public DateTime RaisedOn { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Department { get; set; }
        public string HelpTopic { get; set; }
        public string SubTopic { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public string AssignTo { get; set; }
        public string LastUpdatedBy { get; set; }
		public DateTime? LastModifiedAt { get; set; }
		public string RaisedBy { get; set; }	
	}
}
