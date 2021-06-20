using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <inheritdoc cref="IFeatureFilter" />
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureFilter<TFeature> : IFeatureFilter, IFeatureFilterMetadata<TFeature>
        where TFeature : Enum
    {
        /// <summary>
        /// Evaluates the feature filter to see if the filter's criteria for being enabled has been satisfied.
        /// </summary>
        /// <param name="context">
        /// A feature filter evaluation context that contains information that may be needed to evalute the filter. This
        /// context includes configuration, if any, for this filter for the feature being evaluated.
        /// </param>
        /// <returns><c>true</c> if the filter's criteria has been met; otherwise, <c>false</c>.</returns>
        Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<TFeature> context);
    }
}