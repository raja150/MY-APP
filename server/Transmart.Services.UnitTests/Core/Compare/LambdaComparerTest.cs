using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Compare;
using TranSmart.Domain.Entities.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Core.Compare
{
	public class LambdaComparerTest
	{
		[Fact]
		public void Equals_CheckExpression_ShouldBeTrue()
		{
			var ss = new LambdaComparer<Arrear>((x, y) => x.Pay == y.Pay);
			var response = ss.Equals(new Arrear(), new Arrear());
			Assert.True(response);
		}

		[Fact]
		public void Equals_CheckItems_ShouldBeFalse() 
		{
			var ss = new LambdaComparer<Arrear>((x, y) => x.Pay == y.Pay);
			var response = ss.Equals(null, new Arrear());
			Assert.False(response);
		}

		[Fact]
		public void Equals_CheckItems_ShouldBeTrue()
		{
			var ss = new LambdaComparer<Arrear>((x, y) => x.Pay == y.Pay);
			var response = ss.Equals(null, null);
			Assert.True(response);
		}
	}
}
