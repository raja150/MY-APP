using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_CompOffRequest")]
    public class ApplyCompo : DataGroupEntity
    {
        public Guid? EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid? ApprovedById { get; set; }
        public Organization.Employee ApprovedBy { get; set; }
        public Guid? ShiftId { get; set; }

        public Entities.Leave.Shift Shift { get; set; }
        [StringLength(1024)]
        public string EmailID { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [StringLength(1024)]
        [Required]
        public string ReasonForApply { get; set; }
        public int? Status { get; set; }
        [StringLength(1024)]
        public string AdminReason { get; set; }
    }
}
