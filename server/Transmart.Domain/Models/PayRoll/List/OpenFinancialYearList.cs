using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Models.Payroll
{
    public class OpenFinancialYearList : BaseModel
    {

        public Guid PaySettingsId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string Name { get; set; }
        public bool Closed { get; set; }

    }
}
