using System;

namespace TranSmart.Domain.Models.Cache
{
    public class RoleReportPrivilegeCache
    {
        public Guid RoleId { get; set; }
        public Guid ModuleId { get; set; } 
		public string Module { get; set; }
		public string ModuleLabel { get; set; }
        public Guid ReportId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public bool CanView { get; set; }
		public int DisplayOrder { get; set; }
	}
}
