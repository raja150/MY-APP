using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class DeclarationModel : BaseModel
    {
        public string No { get; set; }
        public Guid PaySettingId { get; set; }  
        public Guid EmployeeId { get; set; }
        public Guid FinancialYearId { get; set; }
        public bool IsNewRegime { get; set; }
        public ICollection<HraDeclarationModel> HRALines { get; set; }
        public HomeLoanPayModel HomeLoanPay { get; set; }
        public ICollection<LetOutPropertyModel> LetOutPropertyLines { get; set; }
        public ICollection<Section6A80CModel> Section80CLines { get; set; }
        public ICollection<Section6A80DModel> Section80DLines { get; set; }
        public ICollection<Section6AOtherModel> SectionOtherLines { get; set; }
        public PrevEmploymentModel PrevEmployment { get; set; }
        public OtherIncomeModel IncomeSource { get; set; }
        public int Salary { get; set; }
        public int Perquisites { get; set; }
        public int PreviousEmployment { get; set; }
        public int TotalSalary { get; set; }
        public int Allowance { get; set; }
        public ICollection<DeclarationAllowanceModel> Allowances { get; set; }
        public int Balance { get; set; }
        public int StandardDeduction { get; set; }
        public int TaxOnEmployment { get; set; }
        /// <summary>
        /// Deductions under section 16
        /// </summary>
        public int Deductions { get; set; }
        public int IncomeChargeable { get; set; }
        public int HouseIncome { get; set; }
        public int OtherIncome { get; set; }
        public int GrossTotal { get; set; }
        public int EightyC { get; set; }
        public int EPF { get; set; }
        public int EightyD { get; set; }
        public int OtherSections { get; set; }
        public int Taxable { get; set; }
        public int Tax { get; set; }
        public int Relief { get; set; }
        public int Cess { get; set; }
        public int TaxPayable { get; set; }
        public int TaxPaid { get; set; }
        public int Due { get; set; }
        public string Name { get; set; }
    } 
}
