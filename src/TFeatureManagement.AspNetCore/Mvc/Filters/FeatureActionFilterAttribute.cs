using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

/// <summary>
/// An action filter attribute that can be used to require a set of features to be enabled for an action to be enabled.
/// If the required features are not enabled the registered <see cref="IDisabledActionHandler{TFeature}" /> will be
/// invoked.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class FeatureActionFilterAttribute<TFeature> : Attribute, IFeatureActionFilterMetadata<TFeature>, IOrderedFilter
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an action filter attribute that requires a set of features to be enabled for the actions to be enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled for the action to be enabled.</param>
    public FeatureActionFilterAttribute(params TFeature[] features)
        : this(RequirementType.All, features)
    {
    }

    /// <summary>
    /// Creates an action filter attribute that requires a set of features to be enabled for the actions to be enabled.
    /// </summary>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="features">The features that should be enabled for the action to be enabled.</param>
    public FeatureActionFilterAttribute(RequirementType requirementType, params TFeature[] features)
    {
        Features = features;
        RequirementType = requirementType;
    }

    /// <inheritdoc />
    public IEnumerable<TFeature> Features { get; }

    /// <inheritdoc />
    public RequirementType RequirementType { get; }

    /// <inheritdoc />
    public int Order { get; set; }
}