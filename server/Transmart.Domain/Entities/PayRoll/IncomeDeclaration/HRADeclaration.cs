using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranSmart.Domain.Entities.Payroll
{
	[Table("PS_HRADeclaration")]
	public class HraDeclaration : DataGroupEntity
	{
		private DateTime _rentalFrom;
		private DateTime _rentalTo;
		public Guid DeclarationId { get; set; }
		public Declaration Declaration { get; set; }
		public DateTime RentalFrom
		{
			get { return new DateTime(_rentalFrom.Year, _rentalFrom.Month, 1); }
			set { _rentalFrom = value; }
		}
		public DateTime RentalTo
		{
			get { return new DateTime(_rentalTo.Year, _rentalTo.Month, DateTime.DaysInMonth(_rentalTo.Year, _rentalTo.Month)); }
			set { _rentalTo = value; }
		}
		public int Amount { get; set; }
		public int Total { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Pan { get; set; }
		public string LandLord { get; set; }
		 
		public void Update(HraDeclaration other)
		{
			RentalFrom = other.RentalFrom;
			RentalTo = other.RentalTo;
			Amount = other.Amount;
			Total = other.Total;
			City = other.City;
			Pan = other.Pan;
			Address = other.Address;
		}
	}
}
