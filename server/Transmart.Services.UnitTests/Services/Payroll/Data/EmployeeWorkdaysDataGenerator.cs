using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll.Data
{
	public class ActiveEmployeeEmployeeWorkdaysDataGenerator : TheoryData<decimal, DateTime, DateTime, DateTime, decimal>
	{
		public ActiveEmployeeEmployeeWorkdaysDataGenerator()
		{
			//decimal monthDays, DateTime startDate, DateTime endDate, DateTime dateOfJoining 
			Add(30, new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), new DateTime(2022, 3, 31), 30);
			Add(30, new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), new DateTime(2022, 4, 1), 30);
			Add(30, new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), new DateTime(2022, 1, 30), 30);
			Add(30, new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), new DateTime(2022, 4, 15), 16);
			Add(30, new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), new DateTime(2022, 4, 30), 1);
			Add(30, new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), new DateTime(2022, 5, 1), 0);

			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 1, 30), 31);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 4, 15), 11);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 4, 30), 0);

			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2023, 4, 30), 0);
		}
	}

	public class ResignedEmployeeEmployeeWorkdaysDataGenerator : TheoryData<decimal, DateTime, DateTime, DateTime, DateTime?, decimal>
	{
		public ResignedEmployeeEmployeeWorkdaysDataGenerator()
		{
			//decimal monthDays, DateTime startDate, DateTime endDate, DateTime dateOfJoining, DateTime? isResigned
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 1, 30), new DateTime(2022, 5, 30), 31);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 1, 30), new DateTime(2022, 4, 25), 31);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 1, 30), new DateTime(2022, 4, 24), 30);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), 31);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 4, 1), new DateTime(2022, 4, 10), 10);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 4, 1), new DateTime(2022, 4, 25), 25);
			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 4, 1), new DateTime(2022, 4, 30), 25);

			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2022, 1, 1), new DateTime(2022, 3, 25), 0);

			Add(31, new DateTime(2022, 3, 26), new DateTime(2022, 4, 25), new DateTime(2023, 4, 30), new DateTime(2023, 4, 30), 0);
		}
	}
}
