using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.Domain.Entities.DailyEvents;

namespace TranSmart.Data
{
    public partial class TranSmartContext
    {
        public DbSet<DailyEvent> Daily_Events { get; set; }
    }
}
