using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

/// <summary>
/// An action filter attribute that can be used to require all or any of a set of features to be enabled for an action
/// to be enabled. If the required features are not enabled the registered
/// <see cref="IDisabledActionHandler{TFeature}" /> will be invoked.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class FeatureActionFilterAttribute<TFeature> : Attribute, IFeatureActionFilterMetadata<TFeature>, IOrderedFilter
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an action filter that requires all the provided feature(s) to be enabled for the actions to be enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureActionFilterAttribute(params TFeature[] features)
        : this(RequirementType.All, features)
    {
    }

    /// <summary>
    /// Creates an action filter that requires the provided feature(s) to be enabled for the actions to be enabled.
    /// The filter can be configured to require all or any of the provided feature(s) to be enabled.
    /// </summary>
    /// <param name="requirementType">
    /// Specifies whether all or any of the provided features should be enabled.
    /// </param>
    /// <param name="features">The features that should be enabled.</param>
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