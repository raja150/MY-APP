using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
    public class LocationAllocationModel : BaseModel
    {
        public Guid LocationId { get; set; }
        public Guid WeekOffSetupId { get; set; }
    }
}
