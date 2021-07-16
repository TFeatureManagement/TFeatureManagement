using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.DependencyInjection
{
    public interface IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : FeatureMetadataBase<TFeature>, new()
    {
        IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureCategoryMetadata<TFeatureCategory>()
            where TFeatureCategory : struct, Enum;

        IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureLifetimeMetadata<TFeatureLifetime>()
            where TFeatureLifetime : struct, Enum;

        IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureTeamMetadata<TFeatureTeam>()
            where TFeatureTeam : struct, Enum;
    }
}