using Microsoft.FeatureManagement;

namespace TFeatureManagement;

/// <summary>
/// Base class for feature filters with a feature enum type.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public abstract class FeatureFilterBase<TFeature> : IFeatureFilter
    where TFeature : struct, Enum
{
    private readonly IFeatureEnumParser<TFeature> _featureEnumParser;

    /// <summary>
    /// Creates a feature filter with an enum parser to convert a feature name to its matching value in the
    /// specified feature enum.
    /// </summary>
    /// <param name="featureEnumParser">The feature enum parser for the feature enum type.</param>
    protected FeatureFilterBase(IFeatureEnumParser<TFeature> featureEnumParser)
    {
        _featureEnumParser = featureEnumParser;
    }

    /// <inheritdoc cref="EvaluateAsync(FeatureFilterEvaluationContext{TFeature})" />
    public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        if (_featureEnumParser.TryParse(context.FeatureName, true, out TFeature feature))
        {
            return await EvaluateAsync(new FeatureFilterEvaluationContext<TFeature>
            {
                Feature = feature,
                Parameters = context.Parameters
            }).ConfigureAwait(false);
        }

        return false;
    }

    /// <summary>
    /// Evaluates the feature filter to see if the filter's criteria for being enabled has been satisfied.
    /// </summary>
    /// <param name="context">
    /// A feature filter evaluation context that contains information that may be needed to evaluate the filter. This
    /// context includes configuration, if any, for this filter for the feature being evaluated.
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns><c>true</c> if the filter's criteria has been met; otherwise, <c>false</c>.</returns>
    public abstract Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<TFeature> context, CancellationToken cancellationToken = default);
}