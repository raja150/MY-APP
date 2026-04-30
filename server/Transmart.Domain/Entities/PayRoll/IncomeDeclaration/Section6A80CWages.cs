using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_Section6A80CWages")]
    public class Section6A80CWages : DataGroupEntity, IEquatable<Section6A80CWages>
    {
        public Guid DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
        public Guid ComponentId { get; set; }
        public DeductionComponent Component { get; set; }
        public int Amount { get; set; }
        public bool Equals([AllowNull] Section6A80CWages other)
        {
            if (other == null) return false;
            return this.Amount.Equals(other.Amount) &&
                           this.ComponentId.Equals(other.ComponentId);
        }
        public void Update(Section6A80CWages other)
        {
            this.Amount = other.Amount;
        }
    } 
}
