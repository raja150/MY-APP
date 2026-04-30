using System;

namespace TranSmart.Domain.Models.Organization
{
    public partial class EmployeePresentAdList : BaseModel
    {
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string CityOrTown { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
