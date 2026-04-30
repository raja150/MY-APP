using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
   public class SalaryModel:BaseModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public string MobileNumber { get; set; }    
        public Guid TemplateId { get; set; }
        public decimal Annually { get; set; }
        public decimal CTC { get; set; }
        public decimal Monthly { get; set; }
        public ICollection<SalaryEarningModel> Earnings { get; set; }    
        public ICollection<SalaryDeductionModel> Deductions { get; set; }   

    }
}
