using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints;

public interface IFeatureActionConstraintFactory<TFeature>
    where TFeature : struct, Enum
{
    IActionConstraint CreateInstance(IFeatureActionConstraintMetadata<TFeature> constraintMetadata);
}