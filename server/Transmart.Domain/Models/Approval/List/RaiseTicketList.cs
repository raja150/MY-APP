using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.SelfService
{
    public partial class RaiseTicketList : BaseModel
    {
        public Guid? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int Status { get; set; }
        public int Category { get; set; }
        public string TicketTitle { get; set; }

    }
}
