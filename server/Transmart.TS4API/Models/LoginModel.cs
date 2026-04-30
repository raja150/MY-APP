using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transmart.TS4API.Models
{
    public class LoginModel
    {
        public string ClientID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
