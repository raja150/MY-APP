using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_WFHRequest")]
    public partial class ApplyWfh : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid? ApprovedById { get; set; }
        public Organization.Employee ApprovedBy { get; set; }
        public Entities.Leave.Shift Shift { get; set; }
        [Required]
        public DateTime FromDateC { get; set; }
        [Required]
        public DateTime ToDateC { get; set; }
		public bool FromHalf { get; set; }
		public bool ToHalf { get; set; }
		[StringLength(1024)]
        [Required]
        public string ReasonForWFH { get; set; }
        public int? Status { get; set; }
        [StringLength(1024)]
        public string AdminReason { get; set; }

    }
}
