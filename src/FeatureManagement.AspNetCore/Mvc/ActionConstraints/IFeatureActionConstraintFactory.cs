using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace FeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    public interface IFeatureActionConstraintFactory<TFeature>
        where TFeature : Enum
    {
        IActionConstraint CreateInstance(IFeatureActionConstraintMetadata<TFeature> constraintMetadata);
    }
}