using System;

namespace TranSmart.Domain.Models.Cache
{
	public class DeskGroupEmpCache : BaseModel
	{
		public string Name { get; set; }
		public string No { get; set; }
		public string Designation { get; set; }
		public string WorkMail { get; set; }
		public Guid	 HdDeptId { get; set; }	
	}
}
