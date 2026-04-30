using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.Approval
{
    public class ClientVisitInfoModel : InfoModel
    {
        public Guid EmployeeId { get; set; }
        public string PlaceOfVisit { get; set; }
        public string PurposeOfVisit { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
