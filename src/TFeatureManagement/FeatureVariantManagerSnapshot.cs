using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public sealed class FeatureVariantManagerSnapshot<TFeature> : IFeatureVariantManagerSnapshot<TFeature>
    where TFeature : struct, Enum
{
    private readonly IVariantFeatureManagerSnapshot _baseFeatureVariantManagerSnapshot;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    public FeatureVariantManagerSnapshot(
        IVariantFeatureManagerSnapshot baseFeatureManagerSnapshot,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _baseFeatureVariantManagerSnapshot = baseFeatureManagerSnapshot ?? throw new ArgumentNullException(nameof(baseFeatureManagerSnapshot));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public async ValueTask<FeatureVariant> GetVariantAsync(TFeature feature, CancellationToken cancellationToken = default)
    {
        var variant = await _baseFeatureVariantManagerSnapshot.GetVariantAsync(_featureNameProvider.GetFeatureName(feature), cancellationToken);

        return new FeatureVariant(variant.Name, variant.Configuration);
    }

    /// <inheritdoc />
    public async ValueTask<FeatureVariant> GetVariantAsync(TFeature feature, ITargetingContext context, CancellationToken cancellationToken = default)
    {
        var targetingContext = new Microsoft.FeatureManagement.FeatureFilters.TargetingContext
        {
            UserId = context.UserId,
            Groups = context.Groups
        };

        var variant = await _baseFeatureVariantManagerSnapshot.GetVariantAsync(_featureNameProvider.GetFeatureName(feature), targetingContext, cancellationToken);

        return new FeatureVariant(variant.Name, variant.Configuration);
    }
}
