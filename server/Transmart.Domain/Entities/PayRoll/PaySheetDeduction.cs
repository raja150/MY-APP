using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
    [Table("PS_PaySheetDeduction")]
    public class PaySheetDeduction : DataGroupEntity, IEquatable<PaySheetDeduction>
    {
        public Guid PaySheetId { get; set; }
        public PaySheet PaySheet { get; set; }
        public string HeaderName { get; set; } 
        /// <summary>
        /// An individual's component amount
        /// </summary>
        public int Salary { get; set; }
        /// <summary>
        /// An individual's component amount with pro-rate
        /// </summary>
        public int Deduction { get; set; }
        public int DeductType { get; set; }
        public Guid ComponentId { get; set; }
        public DeductionComponent Component { get; set; }

        public bool Equals([AllowNull] PaySheetDeduction other)
        {
            if (other == null) return false;

            return this.PaySheetId.Equals(other.PaySheetId) &&
                this.ComponentId.Equals(other.ComponentId) && 
                this.Salary.Equals(other.Salary) &&
                this.Deduction.Equals(other.Deduction)&&
                this.DeductType.Equals(other.DeductType);
        }

        public void Update(PaySheetDeduction other)
        {
            this.PaySheetId = other.PaySheetId;
            this.HeaderName = other.HeaderName; 
            this.Salary = other.Salary;
            this.Deduction = other.Deduction;
            this.ComponentId = other.ComponentId;
            this.DeductType = other.DeductType;
        }
    }
     
}
