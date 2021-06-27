using Microsoft.AspNetCore.Builder;
using System;

namespace TFeatureManagement.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for adding typed middleware that require a feature to be enabled for the middleware to
    /// execute.
    /// </summary>
    public static class UseMiddlewareWhenFeatureEnabledExtensions
    {
        /// <summary>
        /// Conditionally adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <inheritdoc cref="UseMiddlewareExtensions.UseMiddleware{TMiddleware}(IApplicationBuilder, object[])" />
        /// <typeparam name="TFeature">The feature enum type.</typeparam>
        /// <param name="feature">The feature that is required to be enabled for the middleware to execute.</param>
        public static IApplicationBuilder UseMiddlewareWhenFeatureEnabled<TMiddleware, TFeature>(this IApplicationBuilder app, TFeature feature, params object[] args)
            where TFeature : Enum
        {
            return app.UseMiddlewareWhenFeatureEnabled(feature, typeof(TMiddleware), args);
        }

        /// <summary>
        /// Conditionally adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <inheritdoc cref="UseMiddlewareExtensions.UseMiddleware(IApplicationBuilder, Type, object[])" />
        /// <typeparam name="TFeature">The feature enum type.</typeparam>
        /// <param name="feature">The feature that is required to be enabled for the middleware to execute.</param>
        public static IApplicationBuilder UseMiddlewareWhenFeatureEnabled<TFeature>(this IApplicationBuilder app, TFeature feature, Type middleware, params object[] args)
            where TFeature : Enum
        {
            return app.UseWhenFeatureEnabled(feature, builder =>
            {
                builder.UseMiddleware(middleware, args);
            });
        }
    }
}