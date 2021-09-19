using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Example.Models;

namespace TFeatureManagement.AspNetCore.Example.FeatureFilters
{
    public class FeatureFilter : FeatureFilterBase<Feature>
    {
        public FeatureFilter(IFeatureEnumParser<Feature> enumParser)
            : base(enumParser)
        {
        }

        public override Task<bool> EvaluateAsync(FeatureFilterEvaluationContext<Feature> context)
        {
            return Task.FromResult(true);
        }
    }
}