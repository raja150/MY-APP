using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Approval
{
    public class ApproveRequest : BaseModel
    {
        public bool IsApproved { get; set; }
        public string RejectReason { get; set; }
        public string AdminReason { get; set; }
    }
}
