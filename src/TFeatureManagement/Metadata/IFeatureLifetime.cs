using System;

namespace TFeatureManagement.Metadata
{
    public interface IFeatureLifetime<TFeatureLifetime>
        where TFeatureLifetime : struct, Enum
    {
        TFeatureLifetime Lifetime { get; set; }
    }
}