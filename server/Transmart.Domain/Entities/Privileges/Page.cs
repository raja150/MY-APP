using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities
{
    [Table("_Pages")]
    public class Page: DataGroupEntity
    {
        public int SNo { get; set; } 
        public string Module { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int Privilege { get; set; }
    }
}
