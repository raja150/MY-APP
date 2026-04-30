namespace TranSmart.Domain.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations; 
     
    [Table("_AppForms")]
    public class AppForm : DataGroupEntity
    {
        [StringLength(50)]
        [Required]
        [Column(Order = 2)]
        public string Name { get; set; }
        [Column(Order = 3)]
        public string JSON { get; set; }

        [StringLength(100)]
        [Column(Order = 4)]
        [Required]
        public string DisplayName { get; set; }

        [StringLength(50)]
        [Column(Order = 5)]
        [Required]
        public string Header { get; set; }
		[Column(Order = 6)]
		public string SearchJSON { get; set; }	
	}
}
