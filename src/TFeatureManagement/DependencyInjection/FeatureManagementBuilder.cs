using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;

namespace TFeatureManagement.DependencyInjection
{
    public class FeatureManagementBuilder<TFeature> : IFeatureManagementBuilder<TFeature>
        where TFeature : Enum
    {
        private readonly IFeatureManagementBuilder _baseFeatureManagementBuilder;

        public FeatureManagementBuilder(IFeatureManagementBuilder baseFeatureManagementBuilder)
        {
            _baseFeatureManagementBuilder = baseFeatureManagementBuilder;
        }

        public IServiceCollection Services => _baseFeatureManagementBuilder.Services;

        public IFeatureManagementBuilder<TFeature> AddFeatureFilter<T>()
            where T : IFeatureFilterMetadata
        {
            _baseFeatureManagementBuilder.AddFeatureFilter<T>();
            return this;
        }

        public IFeatureManagementBuilder<TFeature> AddSessionManager<T>()
            where T : ISessionManager<TFeature>
        {
            _baseFeatureManagementBuilder.Services.AddSingleton(typeof(T));
            _baseFeatureManagementBuilder.AddSessionManager<TypedSessionManagerExecutor<TFeature, T>>();
            return this;
        }

        public IFeatureManagementBuilder<TFeature> AddFeatureCleanup()
        {
            _baseFeatureManagementBuilder.Services.AddSingleton<IFeatureCleanupManager<TFeature>, FeatureCleanupManager<TFeature>>();
            return this;
        }
    }
}