using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.AppSettings
{
    public partial class Role : DataGroupEntity
    {
        [MaxLength(32)]
        public string Cards { get; set; }
        public string Menu { get; set; }
        public bool CanEdit { get; set; }
        public ICollection<RolePrivilege> Pages { get; set; }//RolePrivileges
        public ICollection<RoleReportPrivilege> Reports { get; set; }
    }
}
