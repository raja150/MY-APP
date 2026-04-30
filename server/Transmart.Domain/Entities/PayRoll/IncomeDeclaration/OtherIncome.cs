using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_OtherIncomeSources")]
	public class OtherIncomeSources : DataGroupEntity
	{
		public Guid DeclarationId { get; set; }
		public Declaration Declaration { get; set; }
		public int OtherSources { get; set; }
		public int InterestOnSaving { get; set; }
		public int InterestOnFD { get; set; }
		public int Qualified()
		{
			return this.OtherSources + this.InterestOnSaving + this.InterestOnFD;
		}
		public void Update(OtherIncomeSources other)
		{
			this.OtherSources = other.OtherSources;
			this.InterestOnSaving = other.InterestOnSaving;
			this.InterestOnFD = other.InterestOnFD;
		}

		public bool NoDataToSave()
		{
			return OtherSources == 0
				&& InterestOnSaving == 0
				&& InterestOnFD == 0;
		}
	}
}
