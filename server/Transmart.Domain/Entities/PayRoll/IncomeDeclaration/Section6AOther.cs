using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_Section6AOther")]
	public class Section6AOther : DataGroupEntity
	{
		public Guid DeclarationId { get; set; }
		public Declaration Declaration { get; set; }
		public Guid OtherSectionsId { get; set; }
		public OtherSections OtherSections { get; set; }
		public int Amount { get; set; }
		public int Qualified { get; set; }
	}
}
