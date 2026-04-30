using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.ExtensionMethods
{
	public static class Extensions
	{
		public static int DayNoInWeek(this DateTime date)
		{
			//Sunday weekno default framework considering it as 0 but in our app we are considering as 7
			return date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;
		}
	}
}
