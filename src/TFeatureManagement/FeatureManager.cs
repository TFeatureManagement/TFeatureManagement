using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public class FeatureManager<TFeature> : IFeatureManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IFeatureManager _baseFeatureManager;
    private readonly IFeatureEnumConverter<TFeature> _featureEnumConverter;

    public FeatureManager(
        IFeatureManager baseFeatureManager,
        IFeatureEnumConverter<TFeature> featureEnumConverter)
    {
        _baseFeatureManager = baseFeatureManager ?? throw new ArgumentNullException(nameof(baseFeatureManager));
        _featureEnumConverter = featureEnumConverter ?? throw new ArgumentNullException(nameof(featureEnumConverter));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.GetFeatureNamesAsync();
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManager.IsEnabledAsync(_featureEnumConverter.GetFeatureName(feature)));
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManager.IsEnabledAsync(_featureEnumConverter.GetFeatureName(feature), context));
    }
}