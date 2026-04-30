using System;

namespace TranSmart.Domain.Models
{
	public class TokenApiModel
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public string Name { get; set; }
		public int Type { get; set; } = 1;
	}
}
