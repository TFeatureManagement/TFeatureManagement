using Microsoft.Extensions.Configuration;

namespace TFeatureManagement;

/// <summary>
/// A context used by <see cref="IFeatureFilter{TFeature}" /> to gain insight into what feature is being evaluated
/// and the parameters needed to check whether the feature should be enabled.
/// </summary>
public class FeatureFilterEvaluationContext<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// The feature being evaluated.
    /// </summary>
    public TFeature Feature { get; set; }

    /// <summary>
    /// The settings provided for the feature filter to use when evaluating whether the feature should be enabled.
    /// </summary>
    public IConfiguration? Parameters { get; set; }
}