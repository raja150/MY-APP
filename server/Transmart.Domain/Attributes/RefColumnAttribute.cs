using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RefColumnAttribute : System.Attribute
    {
        public string ColumnName { get; }
        public RefColumnAttribute(string name)
        {
            this.ColumnName = name;
        }
    }
}
