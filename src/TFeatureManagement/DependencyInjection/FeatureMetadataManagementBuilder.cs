using Microsoft.Extensions.DependencyInjection;
using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.DependencyInjection
{
    internal class FeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> : IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        private readonly IServiceCollection _services;

        public FeatureMetadataManagementBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <inheritdoc />
        public IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureMetadataProvider<TFeatureMetadataProvider>()
            where TFeatureMetadataProvider : class, IFeatureMetadataProvider<TFeature, TFeatureMetadata>
        {
            _services.AddSingleton<IFeatureMetadataProvider<TFeature, TFeatureMetadata>, TFeatureMetadataProvider>();
            return this;
        }
    }
}