using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_PaySheet")]
    public class PaySheet : DataGroupEntity
    {
        public Guid EmployeeID { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid PayMonthId { get; set; }
        public PayMonth PayMonth { get; set; }
        /// <summary>
        /// Workdays of an employee. When an employee resigned or joined workdays are calculated otherwise equal to pay sheet calendar days. 
        /// </summary>
        public decimal WorkDays { get; set; }
        /// <summary>
        /// An employee present days in the office. 
        /// </summary>
        public decimal PresentDays { get; set; }
        /// <summary>
        /// An employee is not present and not taken a leave.
        /// </summary>
        public decimal LOPDays { get; set; }
        /// <summary>
        /// Unauthorized leaves
        /// </summary>
        public decimal UADays { get; set; }
        /// <summary>
        /// Late coming days
        /// </summary>
        public decimal LCDays { get; set; }
        /// <summary>
        /// Salary of an employee defined in each components monthly amount
        /// </summary>
        public int Salary { get; set; }

        /// <summary>
        /// An individual's gross salary includes benefits like HRA, Conveyance Allowance, Medical Allowance, Salary etc
        /// </summary>
        public int Gross { get; set; }
        public int GrossTaxable { get { return Gross - PayCut - LOP - UA - LC; } }
        /// <summary>
        /// An individual's deductions includes tax and statutory components
        /// </summary>
        public int Deduction { get; set; }

        /// <summary>
        /// Net salary is Gross salary - All deductions like Income Tax, Professional Tax, Deductions etc. 
        /// It is also known as take Home Salary
        /// </summary>
        public int Net { get; set; }


        /// <summary>
        /// An individual's incentives from production
        /// </summary>
        public int Incentive { get; set; }

        /// <summary>
        /// Any amount missed or salary hike for salary release month
        /// </summary>
        public int Arrears { get; set; }

        /// <summary>
        /// Loss of pay of an employee
        /// </summary>
        public int LOP { get; set; }
        /// <summary>
        /// Unauthorized days salary deduction
        /// </summary>
        public int UA { get; set; }
        /// <summary>
        /// Late coming days salary deduction
        /// </summary>
        public int LC { get; set; }
        /// <summary>
        /// An individual's deductions and pay cuts from production
        /// </summary>
        public int PayCut { get; set; }

        /// <summary>
        /// EPF calculated gross. Report purpose
        /// </summary>
        public int EPFGross { get; set; }
        /// <summary>
        /// Employee contribution
        /// </summary>
        public int EPF { get; set; }

        /// <summary>
        /// ESI calculated gross. Report purpose
        /// </summary>
        public int ESIGross { get; set; }
        /// <summary>
        /// Employee contribution
        /// </summary>
        public int ESI { get; set; }
        /// <summary>
        /// To know ESI is applied to employee while calculating for next month
        /// </summary>
        public bool ESIApplied { get; set; }

        /// <summary>
        /// State Tax - Professional tax
        /// </summary>
        public int PTax { get; set; }
        /// <summary>
        /// Income Tax
        /// </summary>
        public int Tax { get; set; }
        /// <summary>
        /// Employee salary loan
        /// </summary>
        public int Loan { get; set; }
        public bool Hold { get; set; }
        public string EPFNo { get; set; }
        public string ESINo { get; set; }
		public Guid? DebitBankId { get; set; }
		public Bank DebitBank { get; set; }
		public int PayMode { get; set; }
		public string BankName { get; set; }
        public string BankIFSC { get; set; }
        public string BankACNo { get; set; }
        /// <summary>
        /// Pay sheet status
        /// </summary>
        public int Status { get; set; } 
        public ICollection<PaySheetEarning> Earnings { get; set; }
        public ICollection<PaySheetDeduction> Deductions { get; set; }
        public DateTime? PayslipMailedOn { get; set; }

        public void NoPay()
        {
            Salary = 0;
            Gross = 0;
            Deduction = 0;
            Net = 0;
            Incentive = 0;
            Arrears = 0;
            LOP = 0;
            UA = 0;
            LC = 0;
            PayCut = 0;
            EPFGross = 0;
            EPF = 0;
            ESIGross = 0;
            ESI = 0;
            ESIApplied = false;
            PTax = 0;
            Tax = 0;
            Loan = 0;
            EPFNo = string.Empty;
            ESINo = string.Empty;
            Earnings = new List<PaySheetEarning>();
            Deductions = new List<PaySheetDeduction>();
        }
    }
}
