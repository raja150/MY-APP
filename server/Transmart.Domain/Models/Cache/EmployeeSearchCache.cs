using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Cache
{
    public class EmployeeSearchCache : BaseModel
    {
        public string Name { get; set; }
        public string No { get; set; } 
        public string Department { get; set; }
        public string Designation { get; set; }
		public Guid DepartmentId { get; set; }
        public int Gender { get; set; }
		public string WorkEmail { get; set; }
        public string MobileNumber { get; set; }   
		public string GenderTxt
        {
            get
            {
                return Gender switch
                {
                    1 => "M",
                    _ => "F",
                };
            }
        }
    }
}
