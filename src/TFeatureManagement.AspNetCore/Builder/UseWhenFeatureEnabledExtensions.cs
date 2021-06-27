using Microsoft.AspNetCore.Builder;
using Microsoft.FeatureManagement;
using System;

namespace TFeatureManagement.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for creating a branch in the request pipeline that require a feature to be enabled for the
    /// branch to execute.
    /// </summary>
    public static class UseWhenFeatureEnabledExtensions
    {
        /// <summary>
        /// Conditionally creates a branch in the request pipeline that is rejoined to the main pipeline.
        /// </summary>
        /// <typeparam name="TFeature">The feature enum type.</typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder" /> instance.</param>
        /// <param name="feature">The feature that is required to be enabled for the branch to execute.</param>
        /// <param name="configuration">The configuration for the branch.</param>
        /// <returns>The <see cref="IApplicationBuilder" /> instance.</returns>
        public static IApplicationBuilder UseWhenFeatureEnabled<TFeature>(this IApplicationBuilder app, TFeature feature, Action<IApplicationBuilder> configuration)
            where TFeature : Enum
        {
            return app.UseForFeature(feature.ToString(), configuration);
        }
    }
}