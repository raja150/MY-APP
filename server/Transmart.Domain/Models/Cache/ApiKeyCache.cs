using System;

namespace TranSmart.Domain.Models.Cache
{
	public class ApiKeyCache
	{
		public Guid UserId { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }	
		public Guid RoleId { get; set; }
		public Guid? EmployeeId { get; set; }
	}
}
