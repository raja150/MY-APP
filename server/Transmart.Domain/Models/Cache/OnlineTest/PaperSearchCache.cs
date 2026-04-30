using System;

namespace TranSmart.Domain.Models.Cache.OnlineTest
{
	public class PaperSearchCache
	{
		public Guid PaperId { get; set; }
		public Guid OrganiserId {get; set; }
		public string Paper { get; set; }
	}
}
