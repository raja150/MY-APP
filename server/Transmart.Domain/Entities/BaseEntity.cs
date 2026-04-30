
using System;
using System.ComponentModel.DataAnnotations;

namespace TranSmart.Domain.Entities
{
    public abstract class BaseEntity
    {
		private Guid _id;

		[Key]
		public Guid ID
		{
			get => _id;
			set => _id = value;
		}
	}
    public class AuditEntity : BaseEntity
    {
        public DateTime AddedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        [MaxLength(16)]
        public string CreatedBy { get; set; }
        [MaxLength(16)]
        public string ModifiedBy { get; set; }
    }
    public class AuditLogEntity : BaseEntity
    { 
        public DateTime ModifiedAt { get; set; } 
        [MaxLength(16)]
        public string ModifiedBy { get; set; }
    }
    public class DataGroupEntity : AuditEntity
    {
    }

    public class ValueText
    {
        [Key]
        public Guid Value { get; set; }
        public string Text { get; set; }
    } 
}
