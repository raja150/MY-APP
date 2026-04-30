using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Attributes;

namespace TranSmart.API.Models.Import
{
    public class LeaveBalance
    {
        [DataImport(Name = "Employee Code", Order = 1)]
        public string EmployeeCode { get; set; }

        [DataImport(Name = "Leave Type", Order = 2)]
        public string LeaveType { get; set; }

        [DataImport(Name = "Leaves", Order = 3)]
        public decimal Leaves { get; set; }

        [DataImport(Name = "Reason", Order = 4, Required = false)]
        public string Reason { get; set; }

		[DataImport(Name ="Effective From", Order =5)]
		public DateTime EffectiveFrom { get; set; }

		[DataImport(Name = "Effective To", Order =6)]
		public DateTime EffectiveTo { get; set; }

		[DataImport(Name = "Error", Order = 7, Required = false, ForError = true)]
        public string Error { get; set; }
    }
}
