using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <inheritdoc cref="IContextualFeatureFilter{TContext}" />
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IContextualFeatureFilter<TFeature, TContext> : IContextualFeatureFilter<TContext>, IFeatureFilterMetadata<TFeature>
        where TFeature : Enum
    {
        /// <summary>
        /// Evaluates the feature filter to see if the filter's criteria for being enabled has been satisfied.
        /// </summary>
        /// <param name="context">
        /// A feature filter evaluation context that contains information that may be needed to evalute the filter. This
        /// context includes configuration, if any, for this filter for the feature being evaluated.
        /// </param>
        /// <param name="appContext">
        /// A context defined by the application that is passed in to the feature management system to provide
        /// contextual information for evaluating a feature's state.
        /// </param>
        /// <returns><c>true</c> if the filter's criteria has been met; otherwise, <c>false</c>.</returns>
        Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<TFeature> context, TContext appContext);
    }
}