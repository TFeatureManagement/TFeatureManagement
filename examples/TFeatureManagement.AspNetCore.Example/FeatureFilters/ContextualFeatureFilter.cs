using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Example.Models;

namespace TFeatureManagement.AspNetCore.Example.FeatureFilters
{
    public class ContextualFeatureFilter : ContextualFeatureFilterBase<Feature, ContextualFeatureFilterContext>
    {
        public ContextualFeatureFilter(IFeatureEnumParser<Feature> enumParser)
            : base(enumParser)
        {
        }

        public override Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<Feature> context, ContextualFeatureFilterContext appContext)
        {
            return Task.FromResult(true);
        }
    }
}