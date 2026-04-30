using System;

namespace TranSmart.Domain.Models.Payroll
{
    public partial class BankList : BaseModel
    {
        public string Name { get; set; }
        public string IFSCCode { get; set; }
        public string DisplayName { get; set; }
        public string AccountNo { get; set; }
        public int BankNoLength { get; set; }
        public bool Status { get; set; }
    }
}
