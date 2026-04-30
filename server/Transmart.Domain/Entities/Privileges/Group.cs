using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities
{
    [Table("_Groups")]
    public class Group:DataGroupEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<Page> Pages { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
