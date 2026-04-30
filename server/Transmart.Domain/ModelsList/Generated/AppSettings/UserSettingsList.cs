using System;

namespace TranSmart.Domain.Models.AppSettings
{
    public partial class UserSettingsList : BaseModel
    {
        public int PasswordExpiry { get; set; }
        public int MinimumPassword { get; set; }
        public int MaximumPassword { get; set; }
        public int AllowNumber { get; set; }
    }
}
