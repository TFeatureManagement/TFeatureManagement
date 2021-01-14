using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace FeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    public class FeatureActionConstraintFactory<TFeature> : IFeatureActionConstraintFactory<TFeature>
        where TFeature : Enum
    {
        public IActionConstraint CreateInstance(IFeatureActionConstraintMetadata<TFeature> metadata)
        {
            return new FeatureActionConstraint<TFeature>(metadata.Features, metadata.RequirementType)
            {
                Order = metadata.Order
            };
        }
    }
}