using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
    public class EmployeeActiveAddressModel : BaseModel
    {
     
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string CityOrTown { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string EmpName { get; set; }
    
    }
}
