
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Transmart.Services.UnitTests.Core.Util
{
	public class StringUtilTest
	{
		[Theory]
		[InlineData("vijay", "Vijay")]
		[InlineData("vijay", "vijay")]
		[InlineData("", "")]
		[InlineData("vijayKumar", "VijayKumar")]
		[InlineData("vj Kumar", "VJ Kumar")]

		public void ToCamelCase_ConvertToCamelCase_ShouldBeCamelCase(string Expected, string input)
		{
			var response = TranSmart.Core.Util.StringUtil.ToCamelCase(input);
			Assert.Equal(Expected, response);
		}

		[Fact]
		public void ToLower_CharToLowerCase_ShouldBeLowerCase()
		{
			var response = TranSmart.Core.Util.StringUtil.ToLower('V');
			Assert.Equal('v', response);
		}
	}
}
