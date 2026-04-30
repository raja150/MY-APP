using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class AttendanceStatusModel
    {
        [DataImport(Name = "Employee Code", Order = 1, Required = true)]
        public string EmpCode { get; set; }

        [DataImport(Name = "Attendance Date", Order = 2, Required = true)]
        public DateTime AttendanceDate { get; set; }

        [DataImport(Name = "Attendance Status", Order = 3)]
        public string AttendanceStatus { get; set; }
        [DataImport(Name = "Half Day", Order = 4)]
        public string HalfDay { get; set; }

        [DataImport(Name = "Half Day Status", Order = 5)]
        public string HalfDayType { get; set; }

        [DataImport(Name = "Leave Type(Code)", Order = 6)]
        public string LeaveType { get; set; }

        [DataImport(Name = "Is First Off", Order = 7)]
        public string IsFirstOff { get; set; }
        [DataImport(Name = "Is Unauthorized", Order = 8)]
        public string IsUnauthorized { get; set; }

        [DataImport(Name = "Unauthorized Days", Order = 9)]
        public int? Unauthorized { get; set; }
       
        [DataImport(Name = "Error", Order = 10, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
