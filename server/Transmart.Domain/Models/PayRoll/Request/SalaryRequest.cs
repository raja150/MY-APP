using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Request
{
   public class SalaryRequest:BaseModel
    {
        public Guid EmployeeId { get; set; }
        public Guid? TemplateId { get; set; }
        public decimal Annually { get; set; }
        public decimal CTC { get; set; }
        public decimal Monthly { get; set; } 
        public ICollection<SalaryEarningRequest> Earnings { get; set; }
        public ICollection<SalaryDeductionRequest> Deductions { get; set; }


    }
}
