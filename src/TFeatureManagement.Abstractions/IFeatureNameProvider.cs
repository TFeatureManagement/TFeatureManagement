namespace TFeatureManagement;

/// <summary>
/// Used for getting the name of features.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public interface IFeatureNameProvider<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Gets the name of the feature.
    /// </summary>
    /// <param name="feature">The feature to get the name of.</param>
    /// <returns>The name of the feature.</returns>
    public string GetFeatureName(TFeature feature);
}
