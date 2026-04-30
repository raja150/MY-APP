using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_Salary")]
	public sealed class Salary : DataGroupEntity, IEquatable<Salary>, ICloneable
	{
		public Guid EmployeeId { get; set; }
		public Organization.Employee Employee { get; set; }
		public Guid? TemplateId { get; set; }
		public Template Template { get; set; }
		public int Annually { get; set; }
		public int CTC { get; set; }
		public int Monthly { get; set; }
		public ICollection<SalaryEarning> Earnings { get; set; }
		public ICollection<SalaryDeduction> Deductions { get; set; }

		public object Clone()
		{
			return new Salary
			{
				EmployeeId = this.EmployeeId,
				TemplateId = this.TemplateId,
				Annually = this.Annually,
				CTC = this.CTC,
				Monthly = this.Monthly
			};
		}

		public SalaryHistory AuditObject(Salary item)
		{
			return new SalaryHistory
			{
				SalaryId = item.ID,
				AddedAt = item.AddedAt,
				AnnualCTC = item.Annually,
				CostToCompany = item.CTC,
				CreatedBy = item.CreatedBy,
				EmployeeId = item.EmployeeId,
				TemplateId = item.TemplateId,
				MonthlyCTC = item.Monthly,
			};
		}
		public bool Equals([AllowNull] Salary other)
		{
			if (other == null) return false;

			return this.EmployeeId.Equals(other.EmployeeId)
				&& this.TemplateId.Equals(other.TemplateId)
				&& this.Annually.Equals(other.Annually)
				&& this.CTC.Equals(other.CTC)
				&& this.Monthly.Equals(other.Monthly);
		}

		public void Update(Salary other)
		{
			this.TemplateId = other.TemplateId;
			this.Annually = other.Annually;
			this.Monthly = other.Monthly;
			this.CTC = other.CTC;
		}
	}
}
