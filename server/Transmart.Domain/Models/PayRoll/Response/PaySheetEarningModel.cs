using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll
{
    public class PaySheetEarningModel : BaseModel
    {
        public Guid PaySheetId { get; set; }
        public string HeaderName { get; set; }
        public int HeaderType { get; set; }
        public int Salary { get; set; }
        public int Earning { get; set; }
        public Guid ComponentId { get; set; }
        public int EarningType { get; set; }
    }
}
