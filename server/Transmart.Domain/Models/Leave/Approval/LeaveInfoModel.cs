using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Request;

namespace TranSmart.Domain.Models.Leave.Approval
{
    public class LeaveInfoModel : InfoModel
    {
        public string LeaveTypes { get; set; }
        public string EmergencyContNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool FromHalf { get; set; }
        public bool ToHalf { get; set; }
        public Guid EmployeeId {get;set;}
		public ICollection<ApplyLeaveTypeModel> ApplyLeaveType { get; set; }
	}
}
