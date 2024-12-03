using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureManagerSnapshot<TFeature> : IFeatureManagerSnapshot<TFeature>
    where TFeature : struct, Enum
{
    private readonly IVariantFeatureManagerSnapshot _baseFeatureManagerSnapshot;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    public FeatureManagerSnapshot(
        IVariantFeatureManagerSnapshot baseFeatureManagerSnapshot,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _baseFeatureManagerSnapshot = baseFeatureManagerSnapshot ?? throw new ArgumentNullException(nameof(baseFeatureManagerSnapshot));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManagerSnapshot.GetFeatureNamesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        return _baseFeatureManagerSnapshot.IsEnabledAsync(_featureNameProvider.GetFeatureName(feature), cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        return _baseFeatureManagerSnapshot.IsEnabledAsync(_featureNameProvider.GetFeatureName(feature), context, cancellationToken);
    }
}