using System;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_Result")]
	public class Result : DataGroupEntity
	{
		public Guid EmployeeId { get; set; }
		public Employee Employee { get; set; }
		public int TotalQuestions { get; set; }
		public int TotalTime { get; set; }
		public int TotalMarks { get; set; }
		public Guid PaperId { get; set; }
		public Paper Paper { get; set; }
		public decimal Percentage { get; set; }
		public int Wrong { get; set; }
	}
}
