using System.Collections.Concurrent;

namespace TFeatureManagement;

public class FeatureEnumConverter<TFeature> : IFeatureEnumConverter<TFeature>
    where TFeature : struct, Enum
{
    private readonly ConcurrentDictionary<TFeature, string> _featureNames;

    public FeatureEnumConverter()
    {
        _featureNames = new ConcurrentDictionary<TFeature, string>();
    }

    /// <inheritdoc />
    public string GetFeatureName(TFeature feature)
    {
        return _featureNames.GetOrAdd(feature, feature => feature.ToString());
    }
}
