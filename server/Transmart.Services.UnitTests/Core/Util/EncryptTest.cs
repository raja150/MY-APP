using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Util;
using Xunit;

namespace Transmart.Services.UnitTests.Core.Util
{
	public class EncryptTest
	{
		[Fact]
		public void HashPassword_ToHashPassword_ShouldBeHashCode()
		{
			string text = "Password";
			var response = Encrypt.HashPassword(text);
			Assert.NotEqual(text, response);
		}

		[Fact]
		public void Verify_CheckText_ShouldBeSame()
		{
			string text = "Password";
			string hashCode = "$2a$11$BiODdmVTnsumTYM8l5MoeeBZJpywgaVS4nEiauFKn26.9t3dgjXBm";
			var response = Encrypt.Verify(text, hashCode);
			Assert.True(response);
		}
	}
}
