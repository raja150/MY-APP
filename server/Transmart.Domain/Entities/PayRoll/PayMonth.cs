using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_PayMonth")]

    public class PayMonth : DataGroupEntity
    {
        public Guid FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 0 - Inactive
        /// 1 - Active
        /// 2 - Open
        /// 3 - Inprocess 
        /// 4 - Released
        /// </summary>
        public int Status { get; set; }
        public decimal Days { get; set; }
        public int Cost { get; set; }
        public int Net { get; set; }
        public int Employees { get; set; }
    }
}
