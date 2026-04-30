using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
    public class SalaryEarningRequest : BaseModel
    {
        public Guid SalaryId { get; set; }
        public Guid ComponentId { get; set; }
        public int Type { get; set; }
        public decimal Percentage { get; set; } 
        public Guid? PercentOnCompId { get; set; } 
        public decimal Monthly { get; set; }
        public decimal Annually { get; set; }
        public bool FromTemplate { get; set; }
        public int? PercentOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
