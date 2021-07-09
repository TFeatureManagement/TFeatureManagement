using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    /// <summary>
    /// An interface that identifies a type as metadata for a <see cref="FeatureActionConstraint{TFeature}" />.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureActionConstraintMetadata<TFeature> : IActionConstraintMetadata
        where TFeature : struct, Enum
    {
        /// <summary>
        /// Gets the features that should be enabled.
        /// </summary>
        IEnumerable<TFeature> Features { get; }

        /// <summary>
        /// Gets whether any or all features in <see cref="Features" /> should be enabled.
        /// </summary>
        RequirementType RequirementType { get; }

        /// <inheritdoc cref="IActionConstraint.Order" />
        int Order { get; }
    }
}