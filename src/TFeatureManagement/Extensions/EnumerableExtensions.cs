﻿namespace TFeatureManagement.Extensions;

public static class EnumerableExtensions
{
    public static async ValueTask<bool> AnyAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
    {
        bool enabled = false;

        foreach (TSource item in source)
        {
            if (await predicate(item, cancellationToken).ConfigureAwait(false))
            {
                enabled = true;

                break;
            }
        }

        return enabled;
    }

    public static async ValueTask<bool> AllAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
    {
        bool enabled = true;

        foreach (TSource item in source)
        {
            if (!await predicate(item, cancellationToken).ConfigureAwait(false))
            {
                enabled = false;

                break;
            }
        }

        return enabled;
    }
}