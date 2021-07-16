using System;

namespace TFeatureManagement.Metadata
{
    public abstract class FeatureMetadataBase<TFeature> : IFeatureMetadata<TFeature>
        where TFeature : struct, Enum
    {
        public virtual TFeature Feature { get; set; }
    }
}