namespace TFeatureManagement;

/// <summary>
/// Provides a snapshot of feature state to ensure consistency across a given request.
/// </summary>
public interface IFeatureManagerSnapshot<TFeature> : IFeatureManager<TFeature>
    where TFeature : struct, Enum
{
}