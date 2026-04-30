using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Payroll
{
    public partial class EmpBonus
    {
        public bool Equals(EmpBonus other)
        {
            if (other == null) return false;
            return this.Amount.Equals(other.Amount) &&
              this.ReleasedOn.Equals(other.ReleasedOn);
        }
        public void Update(EmpBonus other)
        {
            this.Amount = other.Amount;
            this.ReleasedOn = other.ReleasedOn;
        }
    }
}
