using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using System.Linq;

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
            var implementationType = typeof(T);
            if (implementationType.GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IFeatureFilterMetadata<>))
                          && i.GetGenericArguments()[0] != typeof(TFeature)))
            {
                throw new ArgumentException($"The feature enum type specified by '{typeof(T)}' must match the feature enum type of the feature management builder.", nameof(T));
            }

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