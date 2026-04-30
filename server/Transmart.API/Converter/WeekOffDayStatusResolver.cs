using AutoMapper;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.List;

namespace TranSmart.API.Converter
{
	public class WeekOffDayStatusResolver : IValueResolver<WeekOffDays, WeekOffDaysList, string>
	{
		public string Resolve(WeekOffDays source, WeekOffDaysList destination, string destMember, ResolutionContext context)
		{
			if (source.Type == 3)
			{
				if (source.Status == 1)
				{
					return "Present";
				}
				else
				{
					return source.Status == 0 ? "Off" : "";
				}
			} 
			return ""; 
		}
	}
}
