using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API.Models
{
    public class PaySlip
    {
        public string MailId { get; set; }
        public string DateOfReport { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public string PFNo { get; set; }
        public string Month { get; set; }
        public string ESINo { get; set; }
        public decimal WorkingDays { get; set; }
        public decimal PresentedDays { get; set; }
        public decimal LeavesTaken { get; set; }
        public int LOPForLeaves { get; set; }

        public decimal LOP { get; set; }
        public int Arrears { get; set; }
        public int ProfessionalTax { get; set; }
        public int PF { get; set; }
        public int ESI { get; set; }
        public int IncomeTax { get; set; }
        public int NetPayble { get; set; }
        public int Incentives { get; set; }
        public int Deductions { get; set; }
        public int GrossEarnings { get; set; }
        public int GrossDeductions { get; set; }																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																				
        public List<Earning> EarningList { get; set; }
        public List<Deductions> DeductionsList { get; set; }
    }
  
    public class Earning
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    public class Deductions
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
