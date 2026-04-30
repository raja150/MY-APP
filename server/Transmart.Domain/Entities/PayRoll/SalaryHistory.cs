using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_SalaryHistory")]
    public class SalaryHistory : DataGroupEntity
    {
        public Guid SalaryId { get; set; }
        public Salary Salary { get; set; }
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid? TemplateId { get; set; }
        public Payroll.Template Template { get; set; }
        public decimal AnnualCTC { get; set; }
        public decimal CostToCompany { get; set; }
        public decimal MonthlyCTC { get; set; }
        public ICollection<SalaryEarningHistory> Earnings { get; set; }
        public ICollection<SalaryDeductionHistory> Deductions { get; set; }
    }   
}
