using System;
using TranSmart.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TranSmart.Domain.Entities.Payroll
{ 
    public partial class FinancialYear  
    {
		public DateTime FromDate { get; set; } 
		public DateTime ToDate  { get; set; }
	}
}
