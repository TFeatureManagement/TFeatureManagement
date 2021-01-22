using Microsoft.AspNetCore.Builder;
using Microsoft.FeatureManagement;
using System;

namespace TFeatureManagement.AspNetCore.Builder
{
    public static class UseForFeatureExtensions
    {
        /// <summary>
        /// Conditionally adds a branch in the application's request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> instance.</param>
        /// <param name="feature">The feature that is required to be enabled to add the branch.</param>
        /// <param name="configuration">
        /// An <see cref="Action" /> to configure the provided <see cref="IApplicationBuilder" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseForFeature<TFeature>(this IApplicationBuilder app, TFeature feature, Action<IApplicationBuilder> configuration)
            where TFeature : Enum
        {
            return app.UseForFeature(feature.ToString(), configuration);
        }
    }
}