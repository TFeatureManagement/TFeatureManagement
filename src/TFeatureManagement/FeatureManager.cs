using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureManager<TFeature> : IFeatureManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IFeatureManager _baseFeatureManager;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    public FeatureManager(
        IFeatureManager baseFeatureManager,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _baseFeatureManager = baseFeatureManager ?? throw new ArgumentNullException(nameof(baseFeatureManager));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.GetFeatureNamesAsync();
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManager.IsEnabledAsync(_featureNameProvider.GetFeatureName(feature)));
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManager.IsEnabledAsync(_featureNameProvider.GetFeatureName(feature), context));
    }
}