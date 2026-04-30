using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using Xunit;

namespace Transmart.Services.UnitTests.Core.Result
{
	public class ResultTest
	{
		[Theory]
		[InlineData("ErrorMessage", true)]
		[InlineData("", false)]
		public void AddMessageItem_AddErrorMessage_ShouldBeTrue(string msg, bool expected)
		{
			MessageItem message = new MessageItem("Test");
			message.Description = msg;

			BaseResult ss = new();

			var response = ss.AddMessageItem(message);
			Assert.Equal(response, expected);
		}
	}
}
