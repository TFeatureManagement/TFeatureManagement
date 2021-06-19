using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <summary>
    /// Base class for contextual feature filters with a feature enum type.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class ContextualFeatureFilterBase<TFeature, TContext> : IContextualFeatureFilter<TFeature, TContext>
        where TFeature : Enum
    {
        private readonly IEnumParser<TFeature> _enumParser;

        /// <summary>
        /// Creates a contextual feature filter with an enum parser to convert a feature name to its matching value in
        /// the specified feature enum.
        /// </summary>
        /// <param name="enumParser">The enum parser for the feature enum type.</param>
        protected ContextualFeatureFilterBase(IEnumParser<TFeature> enumParser)
        {
            _enumParser = enumParser;
        }

        /// <inheritdoc cref="EvaluateAsync(FeatureFilterEvaluationContext{TFeature}, TContext)" />
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context, TContext appContext)
        {
            var feature = _enumParser.Parse(context.FeatureName);

            return EvaluateAsync(new FeatureFilterEvaluationContext<TFeature>
            {
                Feature = feature,
                Parameters = context.Parameters
            }, appContext);
        }

        /// <inheritdoc />
        public abstract Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<TFeature> context, TContext appContext);
    }
}