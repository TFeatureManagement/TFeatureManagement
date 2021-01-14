using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;

namespace FeatureManagement.DependencyInjection
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

        public IFeatureManagementBuilder AddFeatureFilter<T>()
            where T : IFeatureFilterMetadata
        {
            return _baseFeatureManagementBuilder.AddFeatureFilter<T>();
        }

        public IFeatureManagementBuilder AddSessionManager<T>()
            where T : ISessionManager
        {
            return _baseFeatureManagementBuilder.AddSessionManager<T>();
        }
    }
}