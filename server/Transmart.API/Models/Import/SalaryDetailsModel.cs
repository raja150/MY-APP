using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class SalaryDetailsModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImport(Name = "Salary Per Month", Order = 2)]
        public decimal SalaryPerMonth { get; set; }

       
    }
}
