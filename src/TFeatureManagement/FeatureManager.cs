using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureManager<TFeature> : IFeatureManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IFeatureManager _baseFeatureManager;

    public FeatureManager(IFeatureManager baseFeatureManager)
    {
        _baseFeatureManager = baseFeatureManager;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.GetFeatureNamesAsync();
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManager.IsEnabledAsync(feature.ToString()));
    }

    /// <inheritdoc />
    public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(_baseFeatureManager.IsEnabledAsync(feature.ToString(), context));
    }
}