using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_SalaryEarning")]
	public sealed class SalaryEarning : DataGroupEntity, IEquatable<SalaryEarning>
	{
		public Guid SalaryId { get; set; }
		public Salary Salary { get; set; }
		public Guid ComponentId { get; set; }
		public EarningComponent Component { get; set; }
		public int Type { get; set; }
		public int? PercentOn { get; set; }
		public decimal Percentage { get; set; }
		public Guid? PercentOnCompId { get; set; }
		public EarningComponent PercentOnComp { get; set; }
		public int Monthly { get; set; }
		public int Annually { get; set; }
		public bool FromTemplate { get; set; }
		public bool IsDeleted { get; set; }

		public SalaryEarningHistory AuditObject(SalaryEarning item)
		{
			return new SalaryEarningHistory
			{
				ID = Guid.NewGuid(),
				SalaryId = item.SalaryId,
				ComponentId = item.ComponentId,
				Type = item.Type,
				Percentage = item.Percentage,
				Monthly = item.Monthly,
				Annually = item.Annually,
				PercentOnCompId = item.PercentOnCompId,
				CreatedBy = item.CreatedBy,
				AddedAt = item.AddedAt,
				ModifiedAt = item.ModifiedAt,
				ModifiedBy = item.ModifiedBy,
				IsDeleted = item.IsDeleted
			};
		}

		public bool Equals([AllowNull] SalaryEarning other)
		{
			if (other == null) return false;

			return this.ComponentId.Equals(other.ComponentId) &&
				this.Type.Equals(other.Type) &&
				(
					//this.PercentOn != null &&
					this.PercentOn.Equals(other.PercentOn)
				) &&
				this.Percentage.Equals(other.Percentage) &&
				(
					//this.PercentOnCompId != null &&
					this.PercentOnCompId.Equals(other.PercentOnCompId)
				) &&
				this.Monthly.Equals(other.Monthly) &&
				this.Annually.Equals(other.Annually) &&
				this.FromTemplate.Equals(other.FromTemplate);
		}

		public void Update(SalaryEarning other)
		{
			this.ComponentId = other.ComponentId;
			this.Type = other.Type;
			this.PercentOn = other.PercentOn;
			this.Percentage = other.Percentage;
			this.PercentOnCompId = other.PercentOnCompId;
			this.Monthly = other.Monthly;
			this.Annually = other.Annually;
			this.FromTemplate = other.FromTemplate;
			this.IsDeleted = other.IsDeleted;
		}
	}
}
