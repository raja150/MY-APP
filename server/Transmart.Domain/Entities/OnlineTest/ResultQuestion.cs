using System;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_ResultQuestion")]
	public class ResultQuestion : DataGroupEntity
	{
		public Guid QuestionId { get; set; }
		public Question Question { get; set; }
		public bool IsCorrect { get; set; }
		public Guid PaperId { get; set; }
		public Paper Paper { get; set; }
		public int TimeSpent { get; set; }
		public string Answer { get; set; }
		public bool ManuallyCorrected { get; set; }
		/// <summary>
		/// To check who is writing the exam when testEmployee is null
		/// un-able to update the overall result of paper of employee during manual correction
		/// </summary>
		public Guid EmployeeId { get; set; }
		public Employee Employee { get; set; }
		public Guid? TestEmployeeId { get; set; }
		public TestEmployee TestEmployee { get; set; }
		public Guid? TestDesignationId { get; set; }
		public TestDesignation TestDesignation { get; set; }
		public Guid? TestDepartmentId { get; set; }
		public TestDepartment TestDepartment { get; set; }
		public bool ReTake { get; set; }
	}
}
