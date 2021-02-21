using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;

namespace TFeatureManagement.DependencyInjection
{
    public interface IFeatureManagementBuilder<TFeature>
        where TFeature : Enum
    {
        /// <inheritdoc cref="IFeatureManagementBuilder.Services" />
        IServiceCollection Services { get; }

        /// <inheritdoc cref="IFeatureManagementBuilder.AddFeatureFilter{T}" />
        IFeatureManagementBuilder<TFeature> AddFeatureFilter<T>() where T : IFeatureFilterMetadata;

        /// <summary>
        /// Adds an <see cref="ISessionManager{TFeature}" /> to be used for storing feature state in a session.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="ISessionManager{TFeature}" />.</typeparam>
        /// <returns>The feature management builder.</returns>
        IFeatureManagementBuilder<TFeature> AddSessionManager<T>() where T : ISessionManager<TFeature>;
    }
}