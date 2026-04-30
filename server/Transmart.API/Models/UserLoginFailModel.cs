using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API.Models
{
    public class UserLoginFailModel
    {
        public string UserName { get; set; }
        public DateTime LoginAt { get; set; }
        public string IPAddress { get; set; }
    }
}
