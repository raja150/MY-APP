using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class Section6A80CModel : BaseModel
    { 
        public Guid DeclarationId { get; set; } 
        public Guid Section80CId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; } 
    }
}
