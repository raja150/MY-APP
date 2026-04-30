using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Transmart.Controller.UnitTests.ClosedXml
{
	public class ClosedXmlGenericTest
	{
		[Fact]
		public void ValidateHeaders()
		{
			string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
			string filePath = Path.Combine(path, "ClosedXML");
			string file = Directory.GetFiles(filePath).FirstOrDefault(x => x.EndsWith("UnitTest.xlsx"));
			var stream = File.OpenRead(file);
			var test = new TestService();
			bool res = test.ValidateHeaders(stream);
			if (res)
			{
				IEnumerable<TestModel> list = test.ToModel(stream);
				if (list.Count() != 6)
				{
					res = false;
				}
			}
			Assert.True(res);
		}

	}
}
