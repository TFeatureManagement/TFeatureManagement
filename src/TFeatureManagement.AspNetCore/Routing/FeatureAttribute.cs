namespace TFeatureManagement.AspNetCore.Routing;

/// <summary>
/// Attribute for providing feature metadata to be used during routing.
/// </summary>
/// <typeparam name="TFeature"></typeparam>
public class FeatureAttribute<TFeature> : IFeatureMetadata<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an attribute that requires all the provided feature(s) to be enabled for the endpoint to be valid to be
    /// selected.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureAttribute(params TFeature[] features)
        : this(RequirementType.All, features)
    {
    }

    /// <summary>
    /// Creates an attribute that requires the provided feature(s) to be enabled for the action to be valid to be
    /// selected. The constraint can be configured to require all or any of the provided feature(s) to be enabled.
    /// </summary>
    /// <param name="requirementType">
    /// Specifies whether all or any of the provided features should be enabled.
    /// </param>
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
