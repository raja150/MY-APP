using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("SS_ClientVisitRequest")]
    public partial class ApplyClientVisits : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public Guid? ApprovedById { get; set; }
        public Organization.Employee ApprovedBy { get; set; }

        [StringLength(1024)]
        [Required]
        public string PlaceOfVisit { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [StringLength(1024)]
        [Required]
        public string PurposeOfVisit { get; set; }
        public int Status { get; set; }
        [StringLength(1024)]
        public string AdminReason { get; set; }
    }
}

