using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities
{
    [Table("LM_AttendanceModifyLogs")]
    public class AttendanceModifyLogs:DataGroupEntity
    { 
        public Guid AttendanceID { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid? FromLeaveTypeID { get; set; }
        public Guid? ToLeaveTypeID { get; set; }
        public int PresentAttStatus { get; set; }
        public int ModifyStatus { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsFirstOff { get; set; }
        public int HalfDayType { get; set; }
    }
}
