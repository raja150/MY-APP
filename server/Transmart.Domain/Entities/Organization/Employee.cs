using System;
using TranSmart.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Leave;
using System.Collections.Generic;

namespace TranSmart.Domain.Entities.Organization
{ 
    public partial class Employee : DataGroupEntity
    { 
        public ICollection<Attendance> Attendances { get; set; }
        public Allocation Allocation { get; set; }
    }
}
