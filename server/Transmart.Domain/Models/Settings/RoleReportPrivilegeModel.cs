using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Settings
{
    public class RoleReportPrivilegeModel : BaseModel
    {
        public Guid RoleId { get; set; }
        public Guid ModuleId { get; set; }
        public string Module { get; set; }
        public Guid ReportId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public bool Privilege { get; set; }
    }
}
