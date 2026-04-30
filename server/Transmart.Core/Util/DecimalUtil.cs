using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Core.Util
{
	public static class DecimalUtil
	{
		public static decimal ValueFromPercent(decimal value, decimal percent)
		{
			return value + Math.Round(value * (percent / 100), 0);
		}
		public static bool Validation(decimal value)
		{
			if (value < 0) { return true; }
			return false;
		}
		public static bool Validation(int value)
		{
			if (value < 0) { return true; }
			return false;
		}
		public static decimal Percentage(int value, int totalValue)
		{
			return decimal.Round((decimal)value * 100 / (decimal)totalValue, 2);
		}
	}
}
