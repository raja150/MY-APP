using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_PaySheetEarning")]
	public sealed class PaySheetEarning : DataGroupEntity, IEquatable<PaySheetEarning>
	{
		public Guid PaySheetId { get; set; }
		public PaySheet PaySheet { get; set; }
		public string HeaderName { get; set; }
		/// <summary>
		/// An individual's component amount of salary for the month. 
		/// </summary>
		public int Salary { get; set; }
		/// <summary>
		/// An individual's component amount with pro-rate
		/// </summary>
		public int Earning { get; set; }
		public Guid ComponentId { get; set; }
		public int EarningType { get; set; }
		public EarningComponent Component { get; set; } 

		public bool Equals(PaySheetEarning other)
		{
			if (other == null) return false;

			return PaySheetId.Equals(other.PaySheetId) &&
				ComponentId.Equals(other.ComponentId) &&
				Salary.Equals(other.Salary) &&
				Earning.Equals(other.Earning) &&
				EarningType.Equals(other.EarningType);
		}

		public void Update(PaySheetEarning other)
		{
			PaySheetId = other.PaySheetId;
			HeaderName = other.HeaderName;
			EarningType = other.EarningType;
			Salary = other.Salary;
			Earning = other.Earning;
			ComponentId = other.ComponentId;
		}
	}

}
