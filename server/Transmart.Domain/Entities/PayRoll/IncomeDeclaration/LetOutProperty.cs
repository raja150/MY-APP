using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_LetOutProperty")]
    public class LetOutProperty : DataGroupEntity
    { 
        public Guid DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
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

        public void Update(LetOutProperty other)
        {
            this.AnnualRentReceived = other.AnnualRentReceived;
            this.MunicipalTaxPaid = other.MunicipalTaxPaid;
            this.NetAnnualValue = other.NetAnnualValue;
            this.StandardDeduction = other.StandardDeduction;
            this.RepayingHomeLoan = other.RepayingHomeLoan;
            this.InterestPaid = other.InterestPaid;
            this.NameOfLender = other.NameOfLender;
            this.LenderPAN = other.LenderPAN;
            this.NetIncome = other.NetIncome; 
        } 
    } 
}
