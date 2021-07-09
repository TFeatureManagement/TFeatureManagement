using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    public interface IFeatureActionConstraintFactory<TFeature>
        where TFeature : struct, Enum
    {
        IActionConstraint CreateInstance(IFeatureActionConstraintMetadata<TFeature> constraintMetadata);
    }
}