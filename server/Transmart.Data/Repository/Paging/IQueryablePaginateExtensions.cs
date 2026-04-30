using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TranSmart.Data.Paging
{
    public static class IQueryablePaginateExtensions
    {
		public static IPaginate<T> PaginateParameters<T>(int index, int size, int from, int count, IList<T> items, int pages)
		{
			var list = new Paginate<T>
			{
				Index = index,
				Size = size,
				From = from,
				Count = count,
				Items = items,
				Pages = (int)Math.Ceiling(count / (double)size)
			};
			return list;
		}
		public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size,
            int from = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((index - from) * size)
                .Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

			var list = PaginateParameters(index, size, from, count, items, (int)Math.Ceiling(count / (double)size));

			return list;
		}
    }
}
