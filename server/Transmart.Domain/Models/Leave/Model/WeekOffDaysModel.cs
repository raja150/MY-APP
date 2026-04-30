using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Leave
{
    public partial class WeekOffDaysModel 
    {
        public DateTime? WeekDate { get; set; }
        public byte? Status { get; set; }
    }
}
