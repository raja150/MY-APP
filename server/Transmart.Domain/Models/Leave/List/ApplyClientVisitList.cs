using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave.List
{
    public class ApplyClientVisitList : BaseModel
    {
        public string Employee { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string PlaceOfVisit { get; set; }
        public string Status { get; set; }
    }
}
