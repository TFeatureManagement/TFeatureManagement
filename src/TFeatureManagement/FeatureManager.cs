using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureManager<TFeature> : IFeatureManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IVariantFeatureManager _baseFeatureManager;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;
    private readonly IFeatureDefaultsProvider<TFeature> _featureDefaultProvider;
    private readonly IFeatureDefinitionProvider _featureDefinitionProvider;

    public FeatureManager(
        IVariantFeatureManager baseFeatureManager,
        IFeatureNameProvider<TFeature> featureNameProvider,
        IFeatureDefaultsProvider<TFeature> featureDefaultProvider,
        IFeatureDefinitionProvider featureDefinitionProvider)
    {
        _baseFeatureManager = baseFeatureManager ?? throw new ArgumentNullException(nameof(baseFeatureManager));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
        _featureDefaultProvider = featureDefaultProvider ?? throw new ArgumentNullException(nameof(featureDefaultProvider));
        _featureDefinitionProvider = featureDefinitionProvider ?? throw new ArgumentNullException(nameof(featureDefinitionProvider));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
    {
        return _baseFeatureManager.GetFeatureNamesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        var featureName = _featureNameProvider.GetFeatureName(feature);

        //if (_featureDefaultProvider.TryGetIsEnabledDefault(feature, out bool isEnabledDefault) &&
        //    (await _featureDefinitionProvider.GetFeatureDefinitionAsync(featureName).ConfigureAwait(continueOnCapturedContext: false)) == null)
        //{
        //    return isEnabledDefault;
        //}

        return await _baseFeatureManager.IsEnabledAsync(featureName, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
    {
        var featureName = _featureNameProvider.GetFeatureName(feature);

        //if (_featureDefaultProvider.TryGetIsEnabledDefault(feature, out bool isEnabledDefault) &&
        //    (await _featureDefinitionProvider.GetFeatureDefinitionAsync(featureName).ConfigureAwait(continueOnCapturedContext: false)) == null)
        //{
        //    return isEnabledDefault;
        //}

        return await _baseFeatureManager.IsEnabledAsync(featureName, context, cancellationToken);
    }
}