using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_Section6A80C")]
    public class Section6A80C : DataGroupEntity
    {
        public Guid DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
        public Guid Section80CId { get; set; }
        public Section80C Section80C { get; set; }
        public int Amount { get; set; } 

        public void Update(Section6A80C other)
        {
            this.Amount = other.Amount;
        }
    } 
}
