using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.DependencyInjection
{
    /// <inheritdoc cref="IFeatureManagementBuilder{TFeature}" />
    internal class FeatureManagementBuilder<TFeature> : IFeatureManagementBuilder<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureManagementBuilder _baseFeatureManagementBuilder;

        public FeatureManagementBuilder(IFeatureManagementBuilder baseFeatureManagementBuilder)
        {
            _baseFeatureManagementBuilder = baseFeatureManagementBuilder;
        }

        /// <inheritdoc />
        public IServiceCollection Services => _baseFeatureManagementBuilder.Services;

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddFeatureFilter<T>()
            where T : IFeatureFilterMetadata
        {
            _baseFeatureManagementBuilder.AddFeatureFilter<T>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddSessionManager<T>()
            where T : class, ISessionManager<TFeature>
        {
            _baseFeatureManagementBuilder.Services.AddSingleton(typeof(T));
            _baseFeatureManagementBuilder.AddSessionManager<SessionManagerExecutor<TFeature, T>>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddFeatureMetadataManagement<TFeatureMetadata>()
            where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
        {
            return AddFeatureMetadataManagement<TFeatureMetadata>(builder =>
            {
                builder.AddFeatureMetadataProvider<FeatureDescriptionProvider<TFeature, TFeatureMetadata>>();
                builder.AddFeatureMetadataProvider<FeatureCategoryProvider<TFeature, TFeatureMetadata, FeatureCategory>>();
                builder.AddFeatureMetadataProvider<FeatureLifetimeProvider<TFeature, TFeatureMetadata, FeatureLifetime>>();
            });
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddFeatureMetadataManagement<TFeatureMetadata>(Action<IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>> configure)
            where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
        {
            var featureMetadataManagementBuilder = new FeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>(_baseFeatureManagementBuilder.Services);
            configure(featureMetadataManagementBuilder);
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddFeatureCleanupManagement()
        {
            _baseFeatureManagementBuilder.Services.AddSingleton<IFeatureCleanupManager<TFeature>, FeatureCleanupManager<TFeature>>();
            return this;
        }
    }
}