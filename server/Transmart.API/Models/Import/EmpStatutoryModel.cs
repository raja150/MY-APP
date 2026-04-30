using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class EmpStatutoryModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImport(Name = "Enable PF", Order = 2)]
        public string EnableEPF { get; set; }

        [DataImport(Name = "EPF Acc No", Order = 3)]
        public string EPFAccNo { get; set; }

        [DataImport(Name = "UAN", Order = 4)]
        public string UAN { get; set; }

        [DataImport(Name = "Enable ESI", Order = 5)]
        public string EnableESI { get; set; }

        [DataImport(Name = "ESI Acc No", Order =6)]
        public string ESIAccNo { get; set; }
    }
}
