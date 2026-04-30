using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Type => 1;
		public string City { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
		public string GeoLocation { get; set; }
		public string IpAddress { get; set; }

	}
	public class SsoEmpData
	{
		public Guid EmployeeId { get; set; }
		public int ErrorCode { get; set; }
		public string RefreshToken { get; set; }
		public Guid UserId { get; set; }	
	}

}
