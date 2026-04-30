using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities
{
    [Table("_Report")]
    public class Report : AuditEntity
    {
        public Guid ModuleId { get; set; }
        public ReportModule Module { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }
        public string Path { get; set; }
        public string JSON { get; set; }
    }
}
