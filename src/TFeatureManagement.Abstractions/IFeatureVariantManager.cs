namespace TFeatureManagement;

/// <summary>
/// Used to get the assigned variant of a feature, if any.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public interface IFeatureVariantManager<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Gets the assigned variant for a specific feature.
    /// </summary>
    /// <param name="feature">The feature to evaluate.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A variant assigned to the user based on the feature's configured allocation.</returns>
    ValueTask<FeatureVariant?> GetVariantAsync(TFeature feature, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the assigned variant for a specific feature.
    /// </summary>
    /// <param name="feature">The feature to evaluate.</param>
    /// <param name="context">A context that provides information to evaluate which variant will be assigned to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A variant assigned to the user based on the feature's configured allocation.</returns>
    ValueTask<FeatureVariant?> GetVariantAsync(TFeature feature, ITargetingContext context, CancellationToken cancellationToken = default);
}
