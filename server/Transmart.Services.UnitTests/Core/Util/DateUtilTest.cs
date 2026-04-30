using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Payroll.Data;
using TranSmart.Core;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Core
{
	public class DateUtilTest
	{
		[Theory]
		[InlineData(1900, 1, 1900, 1, 1)]
		[InlineData(1900, 1, 1900, 2, 2)]
		[InlineData(1900, 1, 1900, 6, 6)]
		[InlineData(1900, 1, 1900, 12, 12)]
		[InlineData(1900, 1, 1901, 1, 13)]
		[InlineData(1900, 1, 1901, 2, 14)]
		[InlineData(1900, 1, 1902, 1, 25)]
		public void GetMonthDifference_CalculateDifference_Month(int fromYear, int fromMonth, int toYear, int toMonth, int expected)
		{
			var actual = TranSmart.Core.Util.DateUtil.GetMonthDifference(fromYear, fromMonth, toYear, toMonth);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(23, "1999-08-19")]
		[InlineData(23, "1999-09-19")]
		public void CalculateAge_CalculateAgeWithDOB_Age(int ExpectedAge, DateTime DOB)
		{
			var response = DateUtil.CalculateAge(DOB);
			Assert.Equal(ExpectedAge, response);
		}

		[Theory]
		[InlineData("2022-08-22", 0)]
		[InlineData("2022-08-23", 1)]
		[InlineData("2022-08-24", 2)]
		[InlineData("2022-08-25", 3)]
		[InlineData("2022-08-26", 4)]
		[InlineData("2022-08-27", 5)]
		[InlineData("2022-08-21", 6)]
		public void WeekTest(DateTime date, int ExpectedValue)
		{
			var response = DateUtil.Week(date);
			Assert.Equal(response, ExpectedValue);
		}

		[Fact]
		public void DOBFromAge_CalculateDOB_Age()
		{
			var response = DateUtil.DOBFromAge(23);
			Assert.Equal(new DateTime(DateTime.Now.Year - 23, DateTime.Today.Month, 1), 
				response);
		}

		[Fact]
		public void FYFromDate_FinancialYearFrom_ShouldBeFinancialYear()
		{
			var response = DateUtil.FYFromDate(2022, 04, 1);
			Assert.Equal(DateTime.Parse("2022-04-01"), response);
		}

		[Fact]
		public void FYToDate_FinancialYearTo_ShouldBeFinncialYear()
		{
			var response = DateUtil.FYToDate(2022, 04);
			Assert.Equal(DateTime.Parse("2023-03-31"), response);
		}

	}
}
