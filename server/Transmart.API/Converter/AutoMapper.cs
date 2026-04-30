using AutoMapper;
using System.Collections.Generic;

namespace TranSmart.API.Converter
{
    public class Converter<TSource, TDestination> : ITypeConverter<Data.Paging.Paginate<TSource>, Models.Paginate<TDestination>>
    {
        public Models.Paginate<TDestination> Convert(Data.Paging.Paginate<TSource> source, Models.Paginate<TDestination> destination, ResolutionContext context)
        {
            return new Models.Paginate<TDestination>()
            {
                Items = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.Items), //User mapper to go from "foo" to "bar"
                From = source.From,
                Index = source.Index,
                Size = source.Size,
                Count = source.Count,
                Pages = source.Pages,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext
            };
        }
    }
}
