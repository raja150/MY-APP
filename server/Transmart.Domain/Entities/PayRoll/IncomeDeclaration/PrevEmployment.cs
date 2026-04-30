using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_PrevEmployment")]
    public class PrevEmployment : DataGroupEntity
    {
        public Guid DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
        public int IncomeAfterException { get; set; }
        public int IncomeTax { get; set; }
        public int ProfessionalTax { get; set; }
        public int ProvisionalFund { get; set; }
        public int EncashmentExceptions { get; set; }
        public int Qualified()
        {
            return this.IncomeAfterException + EncashmentExceptions;
        } 
        public void Update(PrevEmployment other)
        {
            this.IncomeAfterException = other.IncomeAfterException;
            this.IncomeTax = other.IncomeTax;
            this.ProfessionalTax = other.ProfessionalTax;
            this.ProvisionalFund = other.ProvisionalFund;
            this.EncashmentExceptions = other.EncashmentExceptions;
        }
    }
}
