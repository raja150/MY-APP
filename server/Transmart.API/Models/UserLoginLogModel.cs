using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API.Models
{
    public class UserLoginLogModel
    {
        public Guid UserId { get; set; }
        public DateTime LoginAt { get; set; }
        public string IPAddress { get; set; }
    }
}
