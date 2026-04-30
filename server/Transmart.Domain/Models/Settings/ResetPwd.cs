using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.AppSettings
{
    public class ResetPwd : BaseModel
    {
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }      
    }
}
