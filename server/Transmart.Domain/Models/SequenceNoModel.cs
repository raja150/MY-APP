using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models
{
    public class SequenceNoModel : BaseModel
    {
        public string EntityName { get; set; }

        public string Attribute { get; set; }

        public int NextNo { get; set; }

        public string Prefix { get; set; }

        public string NextDisplayNo { get; set; }
    }

     
}
