using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class Section6A80DModel : BaseModel
    { 
        public Guid DeclarationId { get; set; } 
        public Guid Section80DId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Limit { get; set; }
		public int Qualified { get; set; }
	}
}
