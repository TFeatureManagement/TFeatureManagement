using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;

namespace TFeatureManagement.DependencyInjection
{
    /// <summary>
    /// Provides a way to customize feature management functionality.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureManagementBuilder<TFeature>
        where TFeature : Enum
    {
        /// <inheritdoc cref="IFeatureManagementBuilder.Services" />
        IServiceCollection Services { get; }

        /// <summary>
        /// Adds a feature filter to the list of feature filters that will be available to enable features during
        /// runtime.
        /// </summary>
        /// <remarks>
        /// Possible feature filter metadata types include <see cref="IFeatureFilter" />, <see
        /// cref="IFeatureFilter{TFeature}" />, <see cref="IContextualFeatureFilter{TContext}" /> and <see
        /// cref="IContextualFeatureFilter{TFeature, TContext}" />. Only one feature filter interface can be implemented
        /// by a single type. For feature filters that specify a feature enum type the feature enum type must match the
        /// feature enum type of the feature management builder.
        /// </remarks>
        /// <typeparam name="T">The feature filter type.</typeparam>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddFeatureFilter<T>()
            where T : IFeatureFilterMetadata;

        /// <summary>
        /// Adds an <see cref="ISessionManager{TFeature}" /> to be used for storing feature state in a session.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="ISessionManager{TFeature}" />.</typeparam>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddSessionManager<T>()
            where T : ISessionManager<TFeature>;
    }
}