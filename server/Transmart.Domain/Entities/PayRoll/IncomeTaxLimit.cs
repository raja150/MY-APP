using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.PayRoll
{
    [Table("PS_IncomeTaxLimit")]
    public sealed class IncomeTaxLimit : DataGroupEntity, IEquatable<IncomeTaxLimit>
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Amount { get; set; }

        public bool Equals(IncomeTaxLimit other)
        {
            if (other == null) return false;
            return this.Amount.Equals(other.Amount);
        }
        public void Update(IncomeTaxLimit other)
        {
            this.Amount = other.Amount;
        }
    }
}
