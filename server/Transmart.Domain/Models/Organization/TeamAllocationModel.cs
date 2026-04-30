using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
    public class TeamAllocationModel : BaseModel
    {
        public Guid TeamId { get; set; }
        public Guid WeekOffSetupId { get; set; }
    }
}
