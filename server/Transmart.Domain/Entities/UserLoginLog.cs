using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities
{
    [Table("UserLoginLog")]
    public class UserLoginLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime LoginAt { get; set; }
        public string IPAddress { get; set; }
    }
}
