using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models
{
    public class Report : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string DisplayName { get; set; } 
        public string Label { get; set; }
		public bool Privilege { get; set; }
	}
}
