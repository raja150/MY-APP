using System;

namespace TranSmart.Domain.Models.OnlineTest
{
	public class ResultSearch : BaseSearch
	{
		public Guid? PaperId { get; set; }
		public Guid? EmployeeId { get; set; }
		public DateTime? TestDate { get; set; }
	}
}
