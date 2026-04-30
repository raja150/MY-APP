using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Util;
using Xunit;

namespace Transmart.Services.UnitTests.Core.Util
{
	public class ConstUtilTest
	{
		[Theory]
		[InlineData("Male", 1)]
		[InlineData("Female", 2)]
		[InlineData("Others", 3)]
		public void Gender_ReturnsGender(string Expected, int GenderType)
		{
			var response = ConstUtil.Gender(GenderType);
			Assert.Equal(Expected, response);
		}

		[Theory]
		[InlineData(1, "Married")]
		[InlineData(2, "Unmarried")]
		[InlineData(3, "Separated")]
		public void MartialStatus_ReturnsMartialStatus(int status, string Expected)
		{
			var response = ConstUtil.MartialStatus(status);
			Assert.Equal(Expected, response);
		}
	}
}
