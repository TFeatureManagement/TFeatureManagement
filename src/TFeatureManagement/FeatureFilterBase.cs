using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <summary>
    /// Base class for feature filters with a feature enum type.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public abstract class FeatureFilterBase<TFeature> : IFeatureFilter
        where TFeature : Enum
    {
        private readonly IEnumParser<TFeature> _enumParser;

        /// <summary>
        /// Creates a feature filter with an enum parser to convert a feature name to its matching value in the
        /// specified feature enum.
        /// </summary>
        /// <param name="enumParser">The enum parser for the feature enum type.</param>
        protected FeatureFilterBase(IEnumParser<TFeature> enumParser)
        {
            _enumParser = enumParser;
        }

        /// <inheritdoc cref="EvaluateAsync(FeatureFilterEvaluationContext{TFeature})" />
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var feature = _enumParser.Parse(context.FeatureName);

            return EvaluateAsync(new FeatureFilterEvaluationContext<TFeature>
            {
                Feature = feature,
                Parameters = context.Parameters
            });
        }

        /// <summary>
        /// Evaluates the feature filter to see if the filter's criteria for being enabled has been satisfied.
        /// </summary>
        /// <param name="context">
        /// A feature filter evaluation context that contains information that may be needed to evalute the filter. This
        /// context includes configuration, if any, for this filter for the feature being evaluated.
        /// </param>
        /// <returns><c>true</c> if the filter's criteria has been met; otherwise, <c>false</c>.</returns>
        public abstract Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<TFeature> context);
    }
}