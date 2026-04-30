using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Core.Result
{
    public class CollectionCompareResult<T>
    {
        public IEnumerable<T> Same { get; set; }
        public IEnumerable<T> Added { get; set; }
        public IEnumerable<T> Deleted { get; set; }
    }
}
