using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class LetOutPropertyModel : BaseModel
    {
        public Guid DeclarationId { get; set; }
        public int AnnualRentReceived { get; set; }
        public int MunicipalTaxPaid { get; set; }
        public int NetAnnualValue { get; set; }
        public int StandardDeduction { get; set; }
        public bool RepayingHomeLoan { get; set; }
        public int InterestPaid { get; set; }
        public int Principle { get; set; }  
        public string NameOfLender { get; set; }
        public string LenderPAN { get; set; }
        public int NetIncome { get; set; }
    }
}
