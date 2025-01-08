namespace TFeatureManagement;

public class FeatureNameProvider<TFeature> : IFeatureNameProvider<TFeature>
    where TFeature : struct, Enum
{
    private readonly Dictionary<TFeature, string> _featureNames;

    public FeatureNameProvider()
    {
        _featureNames = [];

#if NET8_0_OR_GREATER
        var features = Enum.GetValues<TFeature>();
#else
        var features = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>();
#endif
        foreach (var feature in features)
        {
            _featureNames.Add(feature, feature.ToString());
        }
    }

    /// <inheritdoc />
    public string GetFeatureName(TFeature feature)
    {
        return _featureNames[feature];
    }
}
