using AutoMapper;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.List;

namespace TranSmart.API.Converter
{
	public class WeekOffDayTypeResolver : IValueResolver<WeekOffDays, WeekOffDaysList, string>
	{
		public string Resolve(WeekOffDays source, WeekOffDaysList destination, string destMember, ResolutionContext context)
		{
			if (source.Type == 1) { return "Week in month"; }
			else if (source.Type == 2) { return "Week in year"; } 
			else { return source.Type == 3 ? "Week date" : ""; }
		}
	}
}
