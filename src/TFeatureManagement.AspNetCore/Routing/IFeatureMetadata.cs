using System;
using System.Collections.Generic;

namespace TFeatureManagement.AspNetCore.Routing;

/// <summary>
/// Represents feature metadata to be used during routing.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public interface IFeatureMetadata<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Gets the features that should be enabled.
    /// </summary>
    IEnumerable<TFeature> Features { get; }

    /// <summary>
    /// Gets which features in <see cref="Features" /> should be enabled.
    /// </summary>
    RequirementType RequirementType { get; }
}
