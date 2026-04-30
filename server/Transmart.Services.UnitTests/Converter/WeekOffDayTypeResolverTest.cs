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
	public class WeekOffDayTypeResolverTest
	{
		[Theory]
		[InlineData("Week in month", 1)]
		[InlineData("Week in year", 2)]
		[InlineData("Week date", 3)]
		[InlineData("", 4)]
		public void Test(string expected, int type)
		{
			var configuration = new MapperConfiguration(cfg =>
				cfg.CreateMap<TranSmart.Domain.Entities.Leave.WeekOffDays, TranSmart.Domain.Models.Leave.List.WeekOffDaysList>()
				.ForMember(d => d.Type, opt => opt.MapFrom<WeekOffDayTypeResolver>()));

			configuration.AssertConfigurationIsValid();

			var source = new TranSmart.Domain.Entities.Leave.WeekOffDays
			{
				Type = type,
			};
			var mapper = configuration.CreateMapper();
			var result = mapper.Map<TranSmart.Domain.Models.Leave.List.WeekOffDaysList>(source);

			Assert.Equal(expected, result.Type);
		}
	}
}
