namespace TranSmart.Domain.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    [Table("_SequenceNo")]
    public class SequenceNo : BaseEntity
    {
        [StringLength(100)]
        [Required]
        public string EntityName { get; set; }

        [StringLength(100)]
        [Required]
        public string Attribute { get; set; }
         
        [Required]
        public int NextNo { get; set; }

        [StringLength(20)]
        [Required]
        public string Prefix { get; set; }

        [StringLength(50)]
        [Required]
        public string NextDisplayNo { get; set; }
    }

    [Table("_LookUpMaster")]
    public class LookUpMaster : DataGroupEntity
    { 
        public string Name { get; set; }
        public string Code { get; set; }
    }

    [Table("_LookUpValues")]
    public class LookUpValues : DataGroupEntity
    { 
        public string Code { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
    }
}