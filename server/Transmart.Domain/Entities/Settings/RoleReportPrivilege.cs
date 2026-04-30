using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Entities.AppSettings
{
    public class RoleReportPrivilege : AuditEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid ReportId { get; set; }
        public Report Report { get; set; }
        public bool Privilege { get; set; }
    }
}
