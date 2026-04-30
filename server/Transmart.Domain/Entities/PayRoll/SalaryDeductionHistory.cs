using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll 
{
    [Table("PS_SalaryDeductionHistory")]
    public class SalaryDeductionHistory : DataGroupEntity
    {
        public Guid SalaryId { get; set; }
        public Salary Salary { get; set; }
        public Guid DeductionId { get; set; }
        public DeductionComponent Deduction { get; set; }
        public int Monthly { get; set; }
        public bool IsDeleted { get; set; }
    }
}
