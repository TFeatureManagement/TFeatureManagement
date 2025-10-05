namespace TFeatureManagement;

public interface IFeatureDefaultProvider<TFeature>
    where TFeature : struct, Enum
{
    public bool TryGetIsEnabledDefault(TFeature feature, out bool isEnabledDefault);
}
