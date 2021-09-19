using System;

namespace TFeatureManagement.Metadata
{
    public interface IFeatureCategory<TFeatureCategory>
        where TFeatureCategory : struct, Enum
    {
        TFeatureCategory Category { get; set; }
    }
}