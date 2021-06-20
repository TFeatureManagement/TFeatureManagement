using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <summary>
    /// Base class for feature filters with a feature enum type.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public abstract class FeatureFilterBase<TFeature> : IFeatureFilter<TFeature>
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

        /// <inheritdoc />
        public abstract Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<TFeature> context);
    }
}