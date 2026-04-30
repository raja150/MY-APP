using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.Domain.Models;

namespace TranSmart.API.Domain.Models
{
    public class RolePrivilegeModel : BaseModel
    {
        public Guid RoleId { get; set; }
        public Guid PageId { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Privilege { get; set; } 
    }
}
