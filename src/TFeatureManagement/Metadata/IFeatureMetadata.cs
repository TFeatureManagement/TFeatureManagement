using System;

namespace TFeatureManagement.Metadata
{
    public interface IFeatureMetadata<TFeature>
        where TFeature : struct, Enum
    {
        TFeature Feature { get; set; }
    }
}