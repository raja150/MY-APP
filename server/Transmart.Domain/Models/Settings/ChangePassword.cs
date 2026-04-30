using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.AppSettings
{
    public class ChangePassword : BaseModel
    {
        public string Token { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public static int Type => 1;
        public string Name { get; set; }
    }
    public class PasswordExpModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public static int Type => 1;
    }
    public class ForgetPwdModel
    {
        public string UserName { get; set; }    
        public string AppUrl { get; set; }  
    }

}
