using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Entities.AppSettings
{
    public class RolePrivilege : AuditEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid PageId { get; set; }
        public Page Page { get; set; }
        public int Privilege { get; set; }
    }
}
