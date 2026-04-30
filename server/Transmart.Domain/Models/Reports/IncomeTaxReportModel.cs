using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
    public partial class IncomeTaxReportModel : BaseModel
    {
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TaxAmount { get; set; }

        public string PAN { get; set; }
    }
}
