
using System;
using TranSmart.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_LoanDeduction")]
    public partial class LoanDeduction : DataGroupEntity
    {
        public Guid LoanID { get; set; }
        public Loan Loan { get; set; }
        public Guid EmployeeID { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid PayMonthId { get; set; }
        public PayMonth PayMonth { get; set; }
        public int Deducted { get; set; }
        public int Left { get; set; }
    }
}
