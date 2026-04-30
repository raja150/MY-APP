using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_BiometricAttLogs")]
    public class BiometricAttLogs : DataGroupEntity
    {
		public Guid? EmployeeId { get; set; }
		public string EmpCode { get; set; }
		public DateTime AttendanceDate { get; set; }
		public DateTime MovementTime { get; set; }
        public int MovementType { get; set; }// 0 For in 1 for Out
		public byte Type { get; set; }
    }

    [Table("LM_ManualAttLogs")]
    public class ManualAttLogs : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
    }
}
