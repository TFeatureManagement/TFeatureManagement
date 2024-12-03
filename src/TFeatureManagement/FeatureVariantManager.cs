using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureVariantManager<TFeature> : IFeatureVariantManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IVariantFeatureManager _baseFeatureVariantManager;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    public FeatureVariantManager(
        IVariantFeatureManager baseFeatureVariantManager,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _baseFeatureVariantManager = baseFeatureVariantManager ?? throw new ArgumentNullException(nameof(baseFeatureVariantManager));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public async ValueTask<FeatureVariant?> GetVariantAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        var variant = await _baseFeatureVariantManager.GetVariantAsync(_featureNameProvider.GetFeatureName(feature), cancellationToken).ConfigureAwait(false);

        return variant != null ? new FeatureVariant(variant.Name, variant.Configuration) : null;
    }

    /// <inheritdoc />
    public async ValueTask<FeatureVariant?> GetVariantAsync(TFeature feature, ITargetingContext context, CancellationToken cancellationToken = default)
    {
        var targetingContext = new Microsoft.FeatureManagement.FeatureFilters.TargetingContext
        {
            UserId = context.UserId,
            Groups = context.Groups
        };

        var variant = await _baseFeatureVariantManager.GetVariantAsync(_featureNameProvider.GetFeatureName(feature), targetingContext, cancellationToken).ConfigureAwait(false);

        return variant != null ? new FeatureVariant(variant.Name, variant.Configuration) : null;
    }
}
