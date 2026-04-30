using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Entities.Leave
{
    public class LeaveBalancesQry: BaseEntity
    {
        public string Name { get; set; }
        public decimal Leaves { get; set; }


    }
}
