using System;

namespace FeatureManagement
{
    /// <summary>
    /// Provides a snapshot of feature state to ensure consistency across a given request.
    /// </summary>
    public interface IFeatureManagerSnapshot<TFeature> : IFeatureManager<TFeature>
        where TFeature : Enum
    {
    }
}