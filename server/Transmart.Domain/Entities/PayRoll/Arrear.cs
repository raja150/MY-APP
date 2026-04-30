using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_Arrear")]

    public sealed class Arrear : DataGroupEntity, IEquatable<Arrear>
    {
        public Guid EmployeeID { get; set; }
        public Organization.Employee Employee { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Pay { get; set; }
        public bool Equals(Arrear other)
        {
            if (other == null) return false;
            return this.Pay.Equals(other.Pay);
        }
        public void Update(Arrear other)
        {
            this.Pay = other.Pay; 
        }  
    }
}
