using Microsoft.FeatureManagement;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests
{
    [SuppressMessage("General", "RCS1079:Throwing of new NotImplementedException.", Justification = "For unit tests only.")]
    public class FeatureFilterWithMultipleFeatureFilterInterfaces : IFeatureFilter, IContextualFeatureFilter<object>
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext featureFilterContext, object appContext)
        {
            throw new NotImplementedException();
        }
    }
}