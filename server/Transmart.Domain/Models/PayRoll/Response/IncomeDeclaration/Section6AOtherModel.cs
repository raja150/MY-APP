using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class Section6AOtherModel : BaseModel
    { 
        public Guid DeclarationId { get; set; } 
        public Guid OtherSectionsId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
		public int Qualified { get; set; }
	}
}
