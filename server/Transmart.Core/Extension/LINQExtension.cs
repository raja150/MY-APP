using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranSmart.Core.Compare;
using TranSmart.Core.Result;

namespace TranSmart.Core.Extension
{
    public static class LinqExtension
    {
        /// <summary>
        /// Returns all items in the first collection except the ones in the second collection that match the lambda condition
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="listA">The first list</param>
        /// <param name="listB">The second list</param>
        /// <param name="lambda">The filter expression</param>
        /// <returns>The filtered list</returns>
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> listA, IEnumerable<T> listB, Func<T, T, bool> lambda)
        {
            return listA.Except(listB, new LambdaComparer<T>(lambda));
        }

        /// <summary>
        /// Returns all items in the first collection that intersect the ones in the second collection that match the lambda condition
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="listA">The first list</param>
        /// <param name="listB">The second list</param>
        /// <param name="lambda">The filter expression</param>
        /// <returns>The filtered list</returns>
        public static IEnumerable<T> Intersection<T>(this IEnumerable<T> listA, IEnumerable<T> listB, Func<T, T, bool> lambda)
        { 
            return listA.Intersect(listB, new LambdaComparer<T>(lambda));
        }

        // Takes a list and returns all records that have overlapping time ranges.
        public static IEnumerable<T> GetOverlappedTimes<T>(this IEnumerable<T> list, IEnumerable<T> listB, Func<T, DateTime> start, Func<T, DateTime> end)
        {
            // Selects all records that match filter() on left side and returns all records on right side that overlap.
            var overlap = from t1 in list
                          from t2 in listB
                          where !object.Equals(t1, t2) // Don’t match the same object on right side.
                          let in1 = start(t1)
                          let out1 = end(t1)
                          let in2 = start(t2)
                          let out2 = end(t2)
                          where in1 <= out2 && out1 >= in2
                          select t2;
            return overlap;
        }
        public static bool HasDuplicate<T>(this IEnumerable<T> list, Func<T, Guid> keySelector)
        {
            return list.GroupBy(keySelector).Any(g => g.Count() > 1);
        }
        /// <summary>
        /// Get compare of list and return modified, added and can deleted
        /// </summary>
        /// <typeparam name="T">Any class object</typeparam>
        /// <param name="listA">Existing data</param>
        /// <param name="listB">New data</param>
        /// <param name="lambda">Expression to compare</param>
        /// <returns></returns>
        public static CollectionCompareResult<T> Compare<T>(
              this IEnumerable<T> listA, IEnumerable<T> listB, Func<T, T, bool> lambda)
        { 
            var result = new CollectionCompareResult<T>();
            
            if (listA == null && listB == null)
            {
                result.Same = new List<T>();
                result.Added = new List<T>();
                result.Deleted = new List<T>();
            }
            else if (listA == null)
            {
                result.Same = new List<T>();
                result.Added = listB;
                result.Deleted = new List<T>();
            }
            else if (listB == null)
            {
                result.Same = new List<T>();
                result.Added = new List<T>();
                result.Deleted = listA;
            }
            else
            {
                result.Same = listA.Intersection(listB, lambda);
                result.Added = listB.Exclude(listA, lambda);
                result.Deleted = listA.Exclude(listB, lambda).ToList();
            }
            return result;
        }

    }
}
