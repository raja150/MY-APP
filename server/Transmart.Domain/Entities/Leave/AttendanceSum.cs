using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_AttendanceSum")]
    public class AttendanceSum : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; } 
        public byte Month { get; set; }
        public Int16 Year { get; set; }
        public decimal Present { get; set; }
        public decimal LOP { get; set; }
        public decimal Unauthorized { get; set; }
		[Column(TypeName = "decimal(4,2)")]
		public decimal OffDays { get; set; }
		public decimal NoOfLeaves { get; set; }
		public void Update(AttendanceSum other)
		{
			Month= other.Month;
			Year= other.Year;
			Present = other.Present;
			LOP = other.LOP;
			Unauthorized = other.Unauthorized;
			NoOfLeaves = other.NoOfLeaves;
		}
	}
	
}
