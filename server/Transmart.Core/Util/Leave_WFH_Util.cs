using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Core.Util
{
	public class Leave_WFH_Util
	{
		public static bool IsFirstHalf(DateTime ApplyFromDate, DateTime ApplyToDate,bool IsFirstHalf, DateTime Date)
		{
			int NoOfDays = (ApplyToDate - ApplyFromDate).Days + 1;

			if (NoOfDays > 1)
			{
				if (Date == ApplyToDate)
				{
					return true;
				}
				else { return false; }
			}
			else
			{
				if (Date == ApplyFromDate.Date && IsFirstHalf)
				{
					return true;
				}
				else { return false; }
			}
		}
	}
}
