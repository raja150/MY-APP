using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class AttendanceLogsImportModel
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImport(Name = "Attendance Date", Order = 2)]
        public DateTime AttendanceDate { get; set; }

        [DataImport(Name = "InTime", Order = 3)]
        public DateTime InTime { get; set; }

        [DataImport(Name = "OutTime", Order = 4)]
        public DateTime OutTime { get; set; }
        public int? AttendanceStatus { get; set; }
        [DataImport(Name = "Error", Order = 5, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
