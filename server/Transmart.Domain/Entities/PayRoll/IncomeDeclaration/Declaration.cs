using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_Declaration")]
    public class Declaration : DataGroupEntity
    {
        public string No { get; set; }
        public Guid FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public bool IsNewRegime { get; set; }
        public List<HraDeclaration> HRALines { get; set; }
        public HomeLoanPay HomeLoanPay { get; set; }
        public ICollection<LetOutProperty> LetOutPropertyLines { get; set; }
        public ICollection<Section6A80CWages> Section6A80CWagesCLines { get; set; }
        public ICollection<Section6A80C> Section80CLines { get; set; }
        public ICollection<Section6A80D> Section80DLines { get; set; }
        public ICollection<Section6AOther> SectionOtherLines { get; set; }
        public PrevEmployment PrevEmployment { get; set; }
        public OtherIncomeSources IncomeSource { get; set; }
        public int Salary { get; set; }
        public int Perquisites { get; set; }
        public int PreviousEmployment { get; set; }
        public int TotalSalary { get; set; }
        /// <summary>
        /// Allowances to the extent exempt under section 10
        /// </summary>
        public int Allowance { get; set; }
        public ICollection<DeclarationAllowance> Allowances { get; set; }
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
		public int HomeLoanPrincipal { get; set; }
		public int EightyD { get; set; }
        public int OtherSections { get; set; }
        public int Taxable { get; set; }
        public int Tax { get; set; }
        public int Relief { get; set; }
        public int Cess { get; set; }
        public int TaxPayable { get; set; }
        public int TaxPaid { get; set; }
        public int Due { get; set; } 
    }

    [Table("PS_DeclarationAllowance")]
    public class DeclarationAllowance : DataGroupEntity, IEquatable<DeclarationAllowance>
    {
        public Guid DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
        public Guid? ComponentId { get; set; }
        public EarningComponent Component { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }

        public bool Equals([AllowNull] DeclarationAllowance other)
        {
            if (other == null) return false;
            return this.Amount.Equals(other.Amount) &&
                           this.Name.Equals(other.Name);
        }
        public void Update(DeclarationAllowance other)
        {
            this.Name = other.Name;
            this.Amount = other.Amount;
        }
    }
}
