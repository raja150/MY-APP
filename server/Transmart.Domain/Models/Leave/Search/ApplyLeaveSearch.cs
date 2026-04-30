using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Search
{
    public class ApplyLeaveSearch : BaseSearch
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
		public byte IsPlanned { get; set; }
    }
}
