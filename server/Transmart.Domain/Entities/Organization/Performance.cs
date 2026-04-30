using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Organization
{
	[Table("Performance")]
	public class Performance : DataGroupEntity
	{
		public Guid EmployeeId { get; set; }
		public Employee Employee { get; set; }
		public  byte PerformanceType { get; set; }
		public int? WeekNumber { get;set; }
		public int Month { get;set; }
		public int Year { get;set; }
		public DateTime PerformedDate { get; set; }
		public void Update(Performance others)
		{
			EmployeeId = others.EmployeeId;
			PerformanceType	= others.PerformanceType;
			WeekNumber = others.WeekNumber;
			PerformedDate = others.PerformedDate;
			Month = others.PerformedDate.Month;
			Year = others.PerformedDate.Year;
			
		}
	}
}
