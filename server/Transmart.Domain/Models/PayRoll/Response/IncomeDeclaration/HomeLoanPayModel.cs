using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class HomeLoanPayModel : BaseModel
    { 
        public Guid DeclarationId { get; set; }
        public string EmployeeName { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal Principle { get; set; }
        public string NameOfLender { get; set; }
        public string LenderPAN { get; set; }
    }
}
