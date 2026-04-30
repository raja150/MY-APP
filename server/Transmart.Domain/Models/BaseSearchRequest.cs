using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models
{
    public class BaseSearch
    { 
        public string Name { set; get; }
        public int Page { get; set; } = 0; 
        public int Size { get; set; } 
        public string SortBy { get; set; }
        public bool IsDescend { get; set; } 
        public Guid? RefId { get; set; }
    }
}
