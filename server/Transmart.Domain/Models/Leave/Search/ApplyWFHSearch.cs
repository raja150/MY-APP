using System;

namespace TranSmart.Domain.Models.Leave.Search
{
   public class ApplyWfhSearch:BaseSearch
	{
		public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
   }
}
