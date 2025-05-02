namespace TFeatureManagement.AspNetCore.Routing;

/// <summary>
/// Attribute for providing feature metadata to be used during routing.
/// </summary>
/// <typeparam name="TFeature"></typeparam>
public class FeatureAttribute<TFeature> : IFeatureMetadata<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an attribute that requires a set of features to be enabled for the action to be valid to be selected.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureAttribute(params TFeature[] features)
        : this(RequirementType.All, features)
    {
    }

    /// <summary>
    /// Creates an attribute that requires a set of features to be enabled for the action to be valid to be selected.
    /// </summary>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureAttribute(RequirementType requirementType, params TFeature[] features)
    {
        Features = features;
        RequirementType = requirementType;
    }

    /// <inheritdoc />
    public IEnumerable<TFeature> Features { get; set; }

    /// <inheritdoc />
    public RequirementType RequirementType { get; set; }
}
