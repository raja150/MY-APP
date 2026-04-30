using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Core.Util
{
	public static class DateUtil
	{
		public static int CalculateAge(DateTime dateOfBirth)
		{
			int age = DateTime.Now.Year - dateOfBirth.Year;
			if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
			{ age--; }
			return age;
		}

		public static int Week(DateTime date)
		{
			var day = date.DayOfWeek;

			return day switch
			{
				DayOfWeek.Monday => 0,
				DayOfWeek.Tuesday => 1,
				DayOfWeek.Wednesday => 2,
				DayOfWeek.Thursday => 3,
				DayOfWeek.Friday => 4,
				DayOfWeek.Saturday => 5,
				DayOfWeek.Sunday => 6,
				_ => -1,
			};
		}
		public static DateTime DOBFromAge(int age)
		{
			int year = DateTime.Now.Year - age;
			int month = DateTime.Now.Month;
			var dateOfBirth = new DateTime(year, month, 1);
			return dateOfBirth;
		}

		public static DateTime FYFromDate(int year, int fromMonth, int day)
		{ 
			return new DateTime(year, fromMonth, day);
		}

		public static DateTime FYToDate(int year, int fromMonth, int day = 1)
		{
			return new DateTime(year, fromMonth, day).AddMonths(12).AddDays(-1);
		} 
		public static int GetMonthDifference(int fromYear, int fromMonth, int toYear, int toMonth)
		{
			if (toYear == fromYear && fromMonth == toMonth) return 1;
			return (((toYear - fromYear) * 12) + toMonth - fromMonth)+1; 
			 
		}

		public static string GetTimeFromMin(double? minutes)
		{
			return minutes == null ? string.Format("{0:00}:{1:00}", 0, 0) : string.Format("{0:00}:{1:00}", TimeSpan.FromMinutes((double)minutes).Hours, TimeSpan.FromMinutes((double)minutes).Minutes);
		}
		public static DateTime FromDate(int month, int year, int startDate)
		{
			DateTime date = new DateTime(year, month, startDate);
			DateTime fromDate = date.AddMonths(-1);
			return fromDate;
		}

		public static DateTime ToDate(int month, int year, int startDate)
		{
			DateTime date = new DateTime(year, month, startDate);
			DateTime toDate = date.AddDays(-1);
			return toDate;
		}
	}
}
