using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using System.Linq;

namespace TFeatureManagement.DependencyInjection
{
    /// <inheritdoc cref="IFeatureManagementBuilder{TFeature}" />
    internal class FeatureManagementBuilder<TFeature> : IFeatureManagementBuilder<TFeature>
        where TFeature : Enum
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
            var implementationType = typeof(T);

            var featureFilterImplementations = implementationType.GetInterfaces()
                .Where(i => i == typeof(IFeatureFilter) ||
                            (i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IContextualFeatureFilter<>))) ||
                            (i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IFeatureFilterMetadata<>))));

            if (featureFilterImplementations.Count() > 1)
            {
                throw new ArgumentException("A single feature filter cannot implement more than one feature filter interface.", nameof(T));
            }
            else if (featureFilterImplementations
                .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IFeatureFilterMetadata<>)) &&
                          i.GetGenericArguments()[0] != typeof(TFeature)))
            {
                throw new ArgumentException("The feature enum type specified by the feature filter must match the feature enum type of the feature management builder.", nameof(T));
            }

            _baseFeatureManagementBuilder.AddFeatureFilter<T>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddSessionManager<T>()
            where T : ISessionManager<TFeature>
        {
            _baseFeatureManagementBuilder.Services.AddSingleton(typeof(T));
            _baseFeatureManagementBuilder.AddSessionManager<TypedSessionManagerExecutor<TFeature, T>>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder<TFeature> AddFeatureCleanup()
        {
            _baseFeatureManagementBuilder.Services.AddSingleton<IFeatureCleanupManager<TFeature>, FeatureCleanupManager<TFeature>>();
            return this;
        }
    }
}