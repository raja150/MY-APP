using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Domain.Entities.PayRoll
{
	[Table("PS_LateComers")]
	public sealed class Latecomers : DataGroupEntity, IEquatable<Latecomers>
	{
		public Guid EmployeeID { get; set; }
		public Organization.Employee Employee { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public decimal NumberOfDays { get; set; }
		public void Update(Latecomers others)
		{
			NumberOfDays = others.NumberOfDays;
		}
		public bool Equals(Latecomers other)
		{
			if (other == null) return false;
			return this.NumberOfDays.Equals(other.NumberOfDays);
		}
	}
}
