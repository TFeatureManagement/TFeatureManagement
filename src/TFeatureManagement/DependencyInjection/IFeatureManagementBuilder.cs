using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.DependencyInjection
{
    /// <summary>
    /// Provides a way to customize feature management functionality.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureManagementBuilder<TFeature>
        where TFeature : struct, Enum
    {
        /// <inheritdoc cref="IFeatureManagementBuilder.Services" />
        IServiceCollection Services { get; }

        /// <inheritdoc cref="IFeatureManagementBuilder.AddFeatureFilter{T}" />
        IFeatureManagementBuilder<TFeature> AddFeatureFilter<T>()
            where T : IFeatureFilterMetadata;

        /// <summary>
        /// Adds an <see cref="ISessionManager{TFeature}" /> to be used for storing feature state in a session.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="ISessionManager{TFeature}" />.</typeparam>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddSessionManager<T>()
            where T : class, ISessionManager<TFeature>;

        /// <summary>
        /// Adds required feature metadata management services.
        /// </summary>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddFeatureMetadataManagement<TFeatureMetadata>()
            where TFeatureMetadata : FeatureMetadataBase<TFeature>, new();

        /// <summary>
        /// Adds required feature metadata management services.
        /// </summary>
        /// <param name="configure">
        /// An <see cref="Action" /> to configure the <see cref="IFeatureMetadataManagementBuilder{TFeature,
        /// TFeatureMetadata}" />.
        /// </param>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddFeatureMetadataManagement<TFeatureMetadata>(Action<IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>> configure)
            where TFeatureMetadata : FeatureMetadataBase<TFeature>, new();

        /// <summary>
        /// Adds required feature cleanup management services.
        /// </summary>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddFeatureCleanupManagement();
    }
}