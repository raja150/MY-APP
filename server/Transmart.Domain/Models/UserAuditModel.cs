using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models
{
    public class UserAuditModel: BaseModel
    {
        public string Description { get; set; }
        public DateTime DateOfUpdated { get; set; }
        public string UpdatedBy { get; set; }
        


    }
}
