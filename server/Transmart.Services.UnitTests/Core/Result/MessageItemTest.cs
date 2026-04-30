using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using Xunit;

namespace Transmart.Services.UnitTests.Core.Result
{
	public class MessageItemTest
	{
		[Fact]
		public void ToString_AddDescriptionAndSource_MessageWithDescriptionAndSource()
		{
			MessageItem messageItem = new MessageItem("ErrorDescription", FeedbackType.Error, "Not Specified");
			var response = messageItem.ToString(true);
			Assert.Contains("Not Specified", response);
		}

		[Fact]
		public void ToString_AddDescription_ResultWithDescription()
		{
			MessageItem messageItem = new MessageItem("ErrorDescription");
			var response = messageItem.ToString(false);
			Assert.Contains("ErrorDescription", response);
		}

	}
}
