namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints;

/// <summary>
/// An action constraint attribute that can be used to require all or any of a set of features to be enabled for an
/// action to be valid to be selected for the given request.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class FeatureActionConstraintAttribute<TFeature> : Attribute, IFeatureActionConstraintMetadata<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an action constraint attribute that requires all the provided feature(s) to be enabled for the action to
    /// be valid to be selected.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureActionConstraintAttribute(params TFeature[] features)
        : this(RequirementType.All, features)
    {
    }

    /// <summary>
    /// Creates an action constraint attribute that requires the provided feature(s) to be enabled for the action to be
    /// valid to be selected. The constraint can be configured to require all or any of the provided feature(s) to be
    /// enabled.
    /// </summary>
    /// <param name="requirementType">
    /// Specifies whether all or any of the provided features should be enabled.
    /// </param>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureActionConstraintAttribute(RequirementType requirementType, params TFeature[] features)
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