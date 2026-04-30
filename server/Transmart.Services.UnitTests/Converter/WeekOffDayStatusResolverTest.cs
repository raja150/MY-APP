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
	public class WeekOffDayStatusResolverTest
	{
		[Theory]
		[InlineData("Off", 3, 0)]
		[InlineData("Present", 3, 1)]
		[InlineData("", 3, 2)]
		[InlineData("", 2, 0)]
		public void Test(string expected, int type, byte status)
		{
			var configuration = new MapperConfiguration(cfg =>
				cfg.CreateMap<TranSmart.Domain.Entities.Leave.WeekOffDays, TranSmart.Domain.Models.Leave.List.WeekOffDaysList>()
				.ForMember(d => d.Status, opt => opt.MapFrom<WeekOffDayStatusResolver>()));

			configuration.AssertConfigurationIsValid();

			var source = new TranSmart.Domain.Entities.Leave.WeekOffDays
			{
				Status = status,
				Type = type
			};
			var mapper = configuration.CreateMapper();
			var result = mapper.Map<TranSmart.Domain.Models.Leave.List.WeekOffDaysList>(source);

			Assert.Equal(expected, result.Status);
		}
	}
}
