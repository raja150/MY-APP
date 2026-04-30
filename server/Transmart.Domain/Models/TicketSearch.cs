using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models
{
    public class TicketSearch : BaseSearch
    {
        public int? Status { get; set; }
		public Guid? TicketStsId { get; set; }	
	}
}
