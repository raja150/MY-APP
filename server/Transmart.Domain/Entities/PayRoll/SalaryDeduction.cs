using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_SalaryDeduction")]
	public sealed class SalaryDeduction : DataGroupEntity, IEquatable<SalaryDeduction>
	{
		public Guid SalaryId { get; set; }
		public Salary Salary { get; set; }
		public Guid DeductionId { get; set; }
		public DeductionComponent Deduction { get; set; }
		public int Monthly { get; set; }
		public bool IsDeleted { get; set; }

		public SalaryDeductionHistory AuditObject(SalaryDeduction item)
		{
			return new SalaryDeductionHistory
			{
				ID = Guid.NewGuid(),
				SalaryId = item.SalaryId,
				DeductionId = item.DeductionId,
				Monthly = item.Monthly,
				CreatedBy = item.CreatedBy,
				AddedAt = item.AddedAt,
				ModifiedAt = item.ModifiedAt,
				ModifiedBy = item.ModifiedBy,
				IsDeleted = item.IsDeleted
			};
		}

		public bool Equals([AllowNull] SalaryDeduction other)
		{
			if (other == null) return false;

			return this.SalaryId.Equals(other.SalaryId) &&
				this.DeductionId.Equals(other.DeductionId) &&
				this.Monthly.Equals(other.Monthly);
		}
		public void Update(SalaryDeduction other)
		{  
			this.Monthly = other.Monthly;
			this.IsDeleted = other.IsDeleted;
		}
	}
}
