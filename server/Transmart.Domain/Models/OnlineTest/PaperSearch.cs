using System;

namespace TranSmart.Domain.Models.OnlineTest
{
	public class PaperSearch : BaseSearch
	{
		public Guid? PaperId { get; set; }
		public int? Type { get; set; }
		public DateTime? TestDate { get; set; }
		public int? Status { get; set; }
		public int? IsinLive { get; set; }
	}
}
