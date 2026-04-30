using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class BonusModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImport(Name = "Bonus", Order = 2)]
        public int Bonus { get; set; }

        [DataImport(Name = "Released On", Order = 3)]   
        public DateTime ReleasedOn{ get; set; }

        [DataImport(Name = "Error", Order = 4, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
