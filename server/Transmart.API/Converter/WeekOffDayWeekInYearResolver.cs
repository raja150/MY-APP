using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.List;

namespace TranSmart.API.Converter
{
	public class WeekOffDayWeekInYearResolver : IValueResolver<WeekOffDays, WeekOffDaysList, string>
	{
		public string Resolve(WeekOffDays source, WeekOffDaysList destination, string destMember, ResolutionContext context)
		{
			if (source.Type == 2)
			{
				if (source.WeekInYear == 0)
				{
					return "All";
				}
				else if (source.WeekInYear == 1)
				{
					return "On even weeks";
				}
				else
				{
					return source.WeekInYear == 2 ? "On odd weeks" : "";
				}
			}
			return "";
		}
	}
}
