namespace TFeatureManagement;

public interface IFeatureDefaultsProvider<TFeature>
    where TFeature : struct, Enum
{
    public IDictionary<TFeature, bool> IsEnabledDefaults { get; }

    public IDictionary<string, bool> IsEnabledDefaultsByFeatureName { get; }
}
