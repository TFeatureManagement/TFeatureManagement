using System;

namespace TFeatureManagement.Metadata
{
    public interface IFeatureMetadataProvider<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        void CreateFeatureMetadata(FeatureMetadataProviderContext<TFeature, TFeatureMetadata> context);
    }
}