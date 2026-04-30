using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class ArrearModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImport(Name = "Pay", Order = 2)]
        public int Pay { get; set; } 

        [DataImport(Name = "Error", Order = 5, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
