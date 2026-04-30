using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class LoanList : BaseModel
    {
        public string LoanNo { get; set; }
        public string Employee { get; set; }
        public DateTime LoanReleasedOn { get; set; }
        public int LoanAmount { get; set; }
        public DateTime DeductFrom { get; set; }
        public int MonthlyAmount { get; set; }
        public string Notes { get; set; }
        public int? Due { get; set; }
        public bool Status { get; set; }
    }
}
