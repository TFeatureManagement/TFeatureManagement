using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureVariantManagerSnapshot<TFeature> : IFeatureVariantManagerSnapshot<TFeature>
    where TFeature : struct, Enum
{
    private readonly IVariantFeatureManagerSnapshot _baseFeatureVariantManagerSnapshot;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    public FeatureVariantManagerSnapshot(
        IVariantFeatureManagerSnapshot baseFeatureVariantManagerSnapshot,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _baseFeatureVariantManagerSnapshot = baseFeatureVariantManagerSnapshot ?? throw new ArgumentNullException(nameof(baseFeatureVariantManagerSnapshot));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public async ValueTask<FeatureVariant?> GetVariantAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        var variant = await _baseFeatureVariantManagerSnapshot.GetVariantAsync(_featureNameProvider.GetFeatureName(feature), cancellationToken).ConfigureAwait(false);

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

        var variant = await _baseFeatureVariantManagerSnapshot.GetVariantAsync(_featureNameProvider.GetFeatureName(feature), targetingContext, cancellationToken).ConfigureAwait(false);

        return variant != null ? new FeatureVariant(variant.Name, variant.Configuration) : null;
    }
}
