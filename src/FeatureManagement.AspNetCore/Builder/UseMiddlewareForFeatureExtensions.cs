using Microsoft.AspNetCore.Builder;
using System;

namespace FeatureManagement.AspNetCore.Builder
{
    public static class UseMiddlewareForFeatureExtensions
    {
        /// <summary>
        /// Conditionally adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type.</typeparam>
        /// <typeparam name="TFeature">The feature type.</typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder" /> instance.</param>
        /// <param name="feature">
        /// The feature that needs to be enabled to add the middleware type in the application's
        /// request pipeline.
        /// </param>
        /// <param name="args">
        /// The arguments to pass to the middleware type instance's constructor.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMiddlewareForFeature<TMiddleware, TFeature>(this IApplicationBuilder app, TFeature feature, params object[] args)
            where TFeature : Enum
        {
            return app.UseMiddlewareForFeature(feature, typeof(TMiddleware), args);
        }

        /// <summary>
        /// Conditionally adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <typeparam name="TFeature">The feature type.</typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder" /> instance.</param>
        /// <param name="feature">
        /// The feature that needs to be enabled to add the middleware type in the application's
        /// request pipeline.
        /// </param>
        /// <param name="middleware">The middleware type.</param>
        /// <param name="args">
        /// The arguments to pass to the middleware type instance's constructor.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMiddlewareForFeature<TFeature>(this IApplicationBuilder app, TFeature feature, Type middleware, params object[] args)
            where TFeature : Enum
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Create and configure the branch builder right away; otherwise, we would end up
            // running our branch after all the components that were subsequently added to the main
            // builder.
            var branchBuilder = app.New();

            return app.UseForFeature(feature, builder =>
            {
                builder.UseMiddleware(middleware, args);
            });
        }
    }
}