using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public class FeatureManagerSnapshot<TFeature> : IFeatureManagerSnapshot<TFeature>
    where TFeature : struct, Enum
{
    private readonly IFeatureManagerSnapshot _baseFeatureManagerSnapshot;
    private readonly IFeatureEnumConverter<TFeature> _featureEnumConverter;

    public FeatureManagerSnapshot(
        IFeatureManagerSnapshot baseFeatureManagerSnapshot,
        IFeatureEnumConverter<TFeature> featureEnumConverter)
    {
        _baseFeatureManagerSnapshot = baseFeatureManagerSnapshot ?? throw new ArgumentNullException(nameof(baseFeatureManagerSnapshot));
        _featureEnumConverter = featureEnumConverter ?? throw new ArgumentNullException(nameof(featureEnumConverter));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManagerSnapshot.GetFeatureNamesAsync();
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManagerSnapshot.IsEnabledAsync(_featureEnumConverter.GetFeatureName(feature)));
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManagerSnapshot.IsEnabledAsync(_featureEnumConverter.GetFeatureName(feature), context));
    }
}