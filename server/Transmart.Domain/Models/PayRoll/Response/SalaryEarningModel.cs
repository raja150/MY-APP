using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
   public class SalaryEarningModel : BaseModel
    {
        public Guid SalaryId { get; set; }
        public Guid ComponentId { get; set; }
        public string Component { get; set; } 
        public int Type { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public string PercentOnComp { get; set; }
        public Guid? PercentOnCompId { get; set; }  
        public decimal Monthly { get; set; }
        public decimal Annually { get; set; }
        public bool FromTemplate { get; set; }
        public int? PercentOn { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}
