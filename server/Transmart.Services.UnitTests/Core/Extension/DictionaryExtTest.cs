using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TranSmart.Core.Extension;

namespace Transmart.Services.UnitTests.Core
{
	public class DictionaryExtTest
	{
		[Fact]
		public void CheckAndAdd_NewItem_CanAdd()
		{
			var dictionary = new Dictionary<string, string>();

			dictionary.CheckAndAdd("Add", "Test");

			Assert.True(dictionary.ContainsKey("Add"));
		}
		[Fact]
		public void CheckAndAdd_ExistingItem_ShoutNotAdd() 
		{
			var dictionary = new Dictionary<string, string>();

			dictionary.CheckAndAdd("Add", "Test");

			dictionary.CheckAndAdd("Add", "Test");
			Assert.True(dictionary.Count == 1);
		}
	}
}
