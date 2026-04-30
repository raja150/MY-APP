using System;

namespace TranSmart.Domain.Models.Helpdesk.Request
{
	public class DeptTransferModel : BaseModel
	{
		public Guid DepartmentId { get; set; }
		public string Message { get; set; }
		public byte TypeOfLog { get; set; }
	}
}
