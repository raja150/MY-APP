using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities
{
    [Table("UserLoginFail")]
    public class UserLoginFail : BaseEntity
    {
        public string UserName { get; set; }
        public DateTime LoginAt { get; set; }
        public string IPAddress { get; set; }
    }
}
