using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_SalaryEarningHistory")]
    public class SalaryEarningHistory : DataGroupEntity
    {
        public Guid SalaryId { get; set; }
        public Salary Salary { get; set; }
        public Guid ComponentId { get; set; }
        public EarningComponent Component { get; set; }
        public int Type { get; set; }
        public int? PercentOn { get; set; }
        public decimal Percentage { get; set; }
        public Guid? PercentOnCompId { get; set; }
        public EarningComponent PercentOnComp { get; set; }
        public int Monthly { get; set; }
        public int Annually { get; set; }
        public bool FromTemplate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
