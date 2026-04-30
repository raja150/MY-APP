using System;

namespace TranSmart.Domain.Models.Payroll.Response
{
    public class HraDeclarationModel : BaseModel
    {
        public Guid DeclarationId { get; set; } 
        public DateTime RentalFrom { get; set; }
        public DateTime RentalTo { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pan { get; set; }
        public string Landlord { get; set; }
    }
}
