using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Converter;
using Xunit;

namespace Transmart.Services.UnitTests.Converter
{
	public class WeekOffDayWeekInYearResolverTest
	{
		[Theory]
		[InlineData("All", 2, 0)]
		[InlineData("On even weeks", 2, 1)]
		[InlineData("On odd weeks", 2, 2)]
		[InlineData("", 1, 1)] 
		public void Test(string expected, int type, int weekInYear)
		{
			var configuration = new MapperConfiguration(cfg =>
				cfg.CreateMap<TranSmart.Domain.Entities.Leave.WeekOffDays, TranSmart.Domain.Models.Leave.List.WeekOffDaysList>()
				.ForMember(d => d.WeekInYear, opt => opt.MapFrom<WeekOffDayWeekInYearResolver>()));

			configuration.AssertConfigurationIsValid();

			var source = new TranSmart.Domain.Entities.Leave.WeekOffDays
			{
				Type = type,
				WeekInYear = weekInYear,
			};
			var mapper = configuration.CreateMapper();
			var result = mapper.Map<TranSmart.Domain.Models.Leave.List.WeekOffDaysList>(source);

			Assert.Equal(expected, result.WeekInYear);
		}
	}
}
