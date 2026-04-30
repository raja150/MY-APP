using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll
{
    public class DeclarationAllowanceModel:BaseModel
    { 
        public Guid DeclarationId { get; set; } 
        public Guid ComponentId { get; set; } 
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
