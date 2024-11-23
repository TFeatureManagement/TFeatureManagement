using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureManager<TFeature> : IFeatureManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IVariantFeatureManager _baseFeatureManager;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    public FeatureManager(
        IVariantFeatureManager baseFeatureManager,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _baseFeatureManager = baseFeatureManager ?? throw new ArgumentNullException(nameof(baseFeatureManager));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.GetFeatureNamesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.IsEnabledAsync(_featureNameProvider.GetFeatureName(feature), cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.IsEnabledAsync(_featureNameProvider.GetFeatureName(feature), context, cancellationToken);
    }
}