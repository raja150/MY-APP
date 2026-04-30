using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Core.Extension
{
    public static class DictionaryExtension
    {
        public static void CheckAndAdd(this Dictionary<string, string> dictionary, string key, string value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}
