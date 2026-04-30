using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API.Models.Import
{
    public class HeaderModel
    {
        public string DisplayName { get; set; }
        public string Values { get; set; }
        public bool Required { get; set; } = true;
        public string DataType { get; set; }
    }
}
