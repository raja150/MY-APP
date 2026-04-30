using System;
using System.Collections.Generic;
using System.Text;
using TranSmart.API.Domain.Models;
using TranSmart.Domain.Models.Settings;

namespace TranSmart.Domain.Models.AppSettings
{
    public partial class RoleModel : BaseModel
    { 
        public string Cards { get; set; }
        public string Menu { get; set; }
        public bool CanEdit { get; set; }
        public List<RolePrivilegeModel> Pages { get; set; }//Privileges
        public List<RoleReportPrivilegeModel> Reports { get; set; }
    }
}
