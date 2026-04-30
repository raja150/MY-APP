using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports.Search
{
    public class EmployeeContactDetailsSearch : BaseSearch
    {
        public Guid? DepartmentId { get; set; }
        public Guid? DesignationId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<Columns> Columns { get; set; }
    }
}
