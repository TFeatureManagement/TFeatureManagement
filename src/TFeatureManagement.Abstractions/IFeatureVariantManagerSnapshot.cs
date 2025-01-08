namespace TFeatureManagement;

/// <summary>
/// Provides a snapshot of feature variant state to ensure consistency across a given request.
/// </summary>
public interface IFeatureVariantManagerSnapshot<TFeature> : IFeatureVariantManager<TFeature>
    where TFeature : struct, Enum
{
}
