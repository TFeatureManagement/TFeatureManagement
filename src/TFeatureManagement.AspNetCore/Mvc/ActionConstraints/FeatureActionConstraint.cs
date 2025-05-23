﻿using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints;

/// <summary>
/// An action constraint that can be used to require a set of features features to be enabled for an action to be valid
/// to be selected for the given request.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public class FeatureActionConstraint<TFeature> : IActionConstraint
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an action constraint that requires a set of features to be enabled for the action to be valid to be
    /// selected.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureActionConstraint(IEnumerable<TFeature> features)
        : this(features, RequirementType.All)
    {
    }

    /// <summary>
    /// Creates an action constraint that requires a set of features to be enabled for the action to be valid to be
    /// selected.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    /// <param name="requirementType">The requirement type.</param>
    public FeatureActionConstraint(IEnumerable<TFeature> features, RequirementType requirementType)
    {
        if (features?.Any() != true)
        {
            throw new ArgumentNullException(nameof(features));
        }

        Features = features;
        RequirementType = requirementType;
    }

    /// <summary>
    /// Gets the features that should be enabled.
    /// </summary>
    public IEnumerable<TFeature> Features { get; }

    /// <summary>
    /// Gets which features in <see cref="Features" /> should be enabled.
    /// </summary>
    public RequirementType RequirementType { get; }

    /// <inheritdoc />
    public int Order { get; set; }

    /// <inheritdoc />
    public bool Accept(ActionConstraintContext context)
    {
        var featureManager = context.RouteContext.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

        return Task.Run(() => featureManager.IsEnabledAsync(RequirementType, Features).AsTask()).GetAwaiter().GetResult();
    }
}