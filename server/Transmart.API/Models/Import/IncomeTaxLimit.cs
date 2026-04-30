using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class IncomeTaxLimitModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeId { get; set; }

        [DataImport(Name = "Amount", Order = 2)]
        public int Amount { get; set; }

        [DataImport(Name = "Error", Order = 5, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
