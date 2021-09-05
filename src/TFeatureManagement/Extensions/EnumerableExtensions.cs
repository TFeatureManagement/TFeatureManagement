using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.Extensions
{
    public static class EnumerableExtensions
    {
        public static async Task<bool> AnyAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            bool enabled = false;

            foreach (TSource item in source)
            {
                if (await predicate(item).ConfigureAwait(false))
                {
                    enabled = true;

                    break;
                }
            }

            return enabled;
        }

        public static async Task<bool> AllAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            bool enabled = true;

            foreach (TSource item in source)
            {
                if (!await predicate(item).ConfigureAwait(false))
                {
                    enabled = false;

                    break;
                }
            }

            return enabled;
        }
    }
}