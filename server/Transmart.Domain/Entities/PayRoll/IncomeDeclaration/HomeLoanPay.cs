using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_HomeLoanPay")]
	public class HomeLoanPay : DataGroupEntity
	{
		public Guid DeclarationId { get; set; }
		public Declaration Declaration { get; set; }
		public int InterestPaid { get; set; }
		public int Principle { get; set; }
		public string NameOfLender { get; set; }
		public string LenderPAN { get; set; }

		public void Update(HomeLoanPay other)
		{
			this.InterestPaid = other.InterestPaid;
			this.NameOfLender = other.NameOfLender;
			this.LenderPAN = other.LenderPAN;
		}
		public bool NoDataToSave()
		{
			return InterestPaid == 0
			   && Principle == 0;
		}
	}
}
