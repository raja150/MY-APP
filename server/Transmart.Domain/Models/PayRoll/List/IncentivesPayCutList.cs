using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.PayRoll.List
{
  public  class IncentivesPayCutList : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string Year { get; set; }
        public int Incentives { get; set; }
        public int PayCut { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }
}
