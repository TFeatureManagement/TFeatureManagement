using Microsoft.Extensions.DependencyInjection;
using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.DependencyInjection
{
    internal class FeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> : IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : FeatureMetadataBase<TFeature>, new()
    {
        public FeatureMetadataManagementBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureCategoryMetadata<TFeatureCategory>()
            where TFeatureCategory : struct, Enum
        {
            Services.AddSingleton<IFeatureMetadataProvider<TFeature, TFeatureMetadata>, FeatureCategoryProvider<TFeature, TFeatureMetadata, TFeatureCategory>>();
            return this;
        }

        public IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureLifetimeMetadata<TFeatureLifetime>()
            where TFeatureLifetime : struct, Enum
        {
            Services.AddSingleton<IFeatureMetadataProvider<TFeature, TFeatureMetadata>, FeatureLifetimeProvider<TFeature, TFeatureMetadata, TFeatureLifetime>>();
            return this;
        }

        public IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureTeamMetadata<TFeatureTeam>()
            where TFeatureTeam : struct, Enum
        {
            Services.AddSingleton<IFeatureMetadataProvider<TFeature, TFeatureMetadata>, FeatureTeamProvider<TFeature, TFeatureMetadata, TFeatureTeam>>();
            return this;
        }
    }
}