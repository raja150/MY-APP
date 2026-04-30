using TranSmart.Domain.Entities.AppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities
{
    public class User : AuditEntity
    { 
        [MaxLength(16)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; } 
        public int NoOfWrongs { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime ExpireOn { get; set; }
        public int Type { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryAt { get; set; }
        //public DateTime AddedAt { get; set; }
        //public DateTime? ModifiedAt { get; set; }
        //public string CreatedBy { get; set; }
        //public string ModifiedBy { get; set; }

        public Guid? EmployeeID { get; set; }
        public Organization.Employee Employee { get; set; }
    }
}
