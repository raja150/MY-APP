using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_Section6A80D")]
    public class Section6A80D : DataGroupEntity
    { 
        public Guid DeclarationId { get; set; }
        public Declaration Declaration { get; set; } 
        public Guid Section80DId { get; set; }
        public Section80D Section80D { get; set; }
        public int Amount { get; set; }
        public int Qualified { get; set; }

        public void Update(Section6A80D other)
        {
            this.Amount = other.Amount;
            this.Qualified = other.Qualified;
        }
    }
     
}
