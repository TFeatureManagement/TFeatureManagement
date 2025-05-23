﻿using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints;

public class FeatureActionConstraintFactory<TFeature> : IFeatureActionConstraintFactory<TFeature>
    where TFeature : struct, Enum
{
    public IActionConstraint CreateInstance(IFeatureActionConstraintMetadata<TFeature> metadata)
    {
        return new FeatureActionConstraint<TFeature>(metadata.Features, metadata.RequirementType)
        {
            Order = metadata.Order
        };
    }
}