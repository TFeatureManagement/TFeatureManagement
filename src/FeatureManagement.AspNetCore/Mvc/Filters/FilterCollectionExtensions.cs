﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FeatureManagement.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Provides integration points for feature management with MVC Filters.
    /// </summary>
    public static class FilterCollectionExtensions
    {
        /// <summary>
        /// Adds a filter that will only execute during a request if the specified feature is
        /// enabled.
        /// </summary>
        /// <typeparam name="TFilter">
        /// Type representing a <see cref="IAsyncActionFilter" />.
        /// </typeparam>
        /// <param name="filters">The filter collection to add to.</param>
        /// <param name="feature">
        /// The feature that is required to be enabled to trigger the execution of the filter.
        /// </param>
        /// <returns>A <see cref="IFilterMetadata" /> representing the added type.</returns>
        public static IFilterMetadata AddForFeature<TFilter, TFeature>(this FilterCollection filters, TFeature feature)
            where TFilter : IAsyncActionFilter
            where TFeature : Enum
        {
            var filter = new FeatureGatedAsyncActionFilter<TFilter, TFeature>(feature);
            filters.Add(filter);

            return filter;
        }
    }
}