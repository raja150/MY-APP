using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;

namespace TranSmart.Data
{
    public partial class TranSmartContext
    {
        public DbSet<Salary> Payroll_Salary { get; set; }
        public DbSet<SalaryEarning> PayRoll_SalaryEarning { get; set; }     
        public DbSet<SalaryDeduction> PayRoll_SalaryDeduction { get; set; }
        public DbSet<HraDeclaration> PayRoll_HouseRent { get; set; }
        public DbSet<Declaration> PayRoll_Declaration { get; set; }
        public DbSet<Section6A80D> PayRoll_Section6A80D { get; set; }
        public DbSet<Section6A80C> PayRoll_Section6A80C { get; set; }
        public DbSet<Section6A80CWages> PayRoll_Section6A80CWages { get; set; }
        public DbSet<LetOutProperty> PayRoll_LetOutProperty { get; set; }
        public DbSet<HomeLoanPay> PayRoll_RePayingHomeLoan { get; set; }
        public DbSet<Section6AOther> PayRoll_Investment { get; set; }
        public DbSet<PrevEmployment> PayRoll_PrevEmployment { get; set; }
        public DbSet<OtherIncomeSources> PayRoll_SourceIncome { get; set; }
        public DbSet<SalaryHistory> Payroll_SalaryHistory  { get; set; }
        public DbSet<SalaryEarningHistory> Payroll_SalaryEarningHistory { get; set; }   
        public DbSet<SalaryDeductionHistory> Payroll_SalaryDeductionHistory { get; set; }
        public DbSet<PayMonth> Payroll_PayMonth { get; set; }
        public DbSet<PaySheet> Payroll_PaySheet { get; set; }
        public DbSet<PaySheetEarning> Payroll_PaySheetComponent { get;set;}
        public DbSet<LoanDeduction> Payroll_LoanDeduction { get; set; }
        public DbSet<Arrear> Payroll_Arrear { get; set; }       
        public DbSet<IncentivesPayCut> Payroll_IncentivesPayCut { get; set; }
        public DbSet<IncomeTaxLimit> Payroll_IncomeTaxLimit { get; set; }   
		public DbSet<Latecomers> Payroll_LateComers { get; set; }
		public DbSet<EmployeePayInfo> Payroll_EmployeePayInfo { get; set; }

	}
}
