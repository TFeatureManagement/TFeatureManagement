using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    public class FeatureActionConstraint<TFeature> : IActionConstraint
        where TFeature : Enum
    {
        public FeatureActionConstraint(IEnumerable<TFeature> features, RequirementType requirementType)
        {
            if (features?.Any() != true)
            {
                throw new ArgumentNullException(nameof(Features));
            }

            Features = features;
            RequirementType = requirementType;
        }

        public IEnumerable<TFeature> Features { get; }

        public RequirementType RequirementType { get; }

        public int Order { get; set; }

        public bool Accept(ActionConstraintContext context)
        {
            var featureManager = context.RouteContext.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

            return RequirementType == RequirementType.All ?
                Features.All(feature => Task.Run(() => featureManager.IsEnabledAsync(feature)).GetAwaiter().GetResult()) :
                Features.Any(feature => Task.Run(() => featureManager.IsEnabledAsync(feature)).GetAwaiter().GetResult());
        }
    }
}