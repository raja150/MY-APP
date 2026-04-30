using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
    public partial class WeekOffDays
    {
        public DateTime? WeekDate { get; set; }
        public byte? Status { get; set; }
    }
}
