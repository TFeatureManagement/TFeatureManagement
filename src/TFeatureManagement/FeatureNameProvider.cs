namespace TFeatureManagement;

public class FeatureNameProvider<TFeature> : IFeatureNameProvider<TFeature>
    where TFeature : struct, Enum
{
    public FeatureNameProvider()
    {
        FeatureNames = [];

#if NET8_0_OR_GREATER
        var features = Enum.GetValues<TFeature>();
#else
        var features = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>();
#endif
        foreach (var feature in features)
        {
            FeatureNames.Add(feature, feature.ToString());
        }
    }

    internal Dictionary<TFeature, string> FeatureNames { get; private set; }

    /// <inheritdoc />
    public string GetFeatureName(TFeature feature)
    {
        if (!FeatureNames.TryGetValue(feature, out var featureName))
        {
            featureName = feature.ToString();
        }

        return featureName;
    }
}
