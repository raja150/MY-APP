namespace TranSmart.Domain.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("_ReportModule")]
    public class ReportModule : AuditEntity
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }
    }
}