using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Core
{
    public class HeaderModel
    {
        private string _propertyName;
        public string PropertyName
        {
            get { return Util.StringUtil.ToCamelCase(_propertyName); }
            set { _propertyName = value; }
        }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
