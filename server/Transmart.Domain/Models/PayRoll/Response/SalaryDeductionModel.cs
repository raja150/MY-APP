using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response  
{
    public class SalaryDeductionModel : BaseModel
    {
        public Guid SalaryId { get; set; } 
        public Guid DeductionId { get; set; }
        public int Monthly { get; set; }
        public bool IsDeleted { get; set; }
    }
}
