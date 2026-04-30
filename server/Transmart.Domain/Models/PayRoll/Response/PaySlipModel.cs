using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll
{

    public class PaySlipModel
    {
        public Guid EmployeeID { get; set; }
        public Guid PayMonthId { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public decimal Days { get; set; }
        public decimal PresentDays { get; set; }
        public decimal LOPDays { get; set; }
        public decimal UADays { get; set; }
        public int Salary { get; set; }
        public int Gross { get; set; }
        public int Deduction { get; set; }
        public int Net { get; set; }
        public int GrossTaxable { get { return Gross - PayCut - LOP; } }
        public int Incentive { get; set; }
        public int Arrears { get; set; }
        public int LOP { get; set; }
        public int PayCut { get; set; }
        public int EPF { get; set; }
        public int ESI { get; set; }
        public int PTax { get; set; }
        public int Tax { get; set; }
        public int Loan { get; set; }
        public bool Hold { get; set; }
        public string EPFNo { get; set; }
        public string ESINo { get; set; }
        public string BankName { get; set; }
        public string BankIFSC { get; set; }
        public string BankACNo { get; set; }
        public bool ChequePay { get; set; }
        public int Status { get; set; }
        public decimal WorkDays { get; set; }
        public int UA { get; set; }
        public int LC { get; set; }
        public ICollection<PaySheetEarningModel> Earnings { get; set; }
        public ICollection<PaySheetDeductionModel> Deductions { get; set; }
    }
}
