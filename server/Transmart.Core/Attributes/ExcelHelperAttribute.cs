using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TranSmart.Core.Attributes
{
    public class ExcelHelperAttribute
    {
        public DataImportAttribute Attribute { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
    }
}
